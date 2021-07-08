# CLI commands used

## Create the web app project
- `dotnet new webapp -o ContosoUniversity`
- `cd ContosoUniversity`

## Install required NuGet packages
- `dotnet add package Microsoft.EntityFrameworkCore.SQLite -v 5.0.0-*`
- `dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 5.0.0-*`
- `dotnet add package Microsoft.EntityFrameworkCore.Design -v 5.0.0-*`
- `dotnet add package Microsoft.EntityFrameworkCore.Tools -v 5.0.0-*`
- `dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design -v 5.0.0-*`
- `dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore -v 5.0.0-*`

## Globally install the aspnet-codegenerator scaffolding tool
- `dotnet tool install --global dotnet-aspnet-codegenerator`

## Use codegenerator to scaffold Student pages, and wire up db context
- *Create a Pages/Students folder*
- `dotnet aspnet-codegenerator razorpage -m Student -dc ContosoUniversity.Data.SchoolContext -udl -outDir Pages\Students --referenceScriptLibraries -sqlite`
- 10 files will be generated in *Pages/Students* folder
- DB context will be registered as a service in `Startup.ConfigureServices()` (DI container), using SQLite
- `-udl` means `--​useDefaultLayout`

## Migrations for SQLite
- Install Entity Framework CLI `dotnet tool install --global dotnet-ef`
- Automatic `ef database update` will fail for **SQLite** as it doesn't support removing columns
- Need to manually update migrations code to perform a table rebuild.
- Generate the initial migration code `dotnet ef migrations add InitialCreate`
- Updates the code in the `Up` and `Down` methods in InitialCreate
- Drop the old SQLite database **"CU.db"** `dotnet ef database drop --force`
- Execute the migration code to re-create the database **"CU.db"** `dotnet ef database update`

## Update Student model
- Add new property and attributes in In Student.cs
- The attribute `[Column("FirstName")]` for **FirstMidName** will map the name of the FirstMidName property to "FirstName" in the database
- At this point, running the app will fail with *"SqliteException: SQLite Error 1: 'no such column: s.FirstName'."* 
- Create another migration with the updated schema `dotnet ef migrations add ColumnFirstName`
- Execute the new migration `dotnet ef database update`, this will fix the problem
- In SQLite, the column that was FirstMidName is now FirstName.

## Add new entities: Course, Department, Instructor, OfficeAssignment
- Add new entities: Course, Department, Instructor, OfficeAssignment
- Setup Foreign Keys and reference navigation properties in each entity model
- In SchoolContext, use **Fluent API** to configure the many-to-many relationship between the Instructor and Course entities
- Update the seed data in *DbInitializer.cs*, to include new entities

## Drop and re-create the database
- If using SQLite, need to drop and recreate the database, migrations history will be cleared
- Delete the *Migrations* folder
- Drop the old SQLite db `dotnet ef database drop --force`, *CU.db* will be removed
- Generate new migration files `dotnet ef migrations add InitialCreate`
- Update the database by executing the migration files `dotnet ef database update`, *CU.db* will be re-created
- Run the app `dotnet watch run`, the empty database will be seeded with new entities

## Create Course pages
- Scafold Course pages `dotnet aspnet-codegenerator razorpage -m Course -dc SchoolContext -udl -outDir Pages\Courses --referenceScriptLibraries`
- 10 files will be generated in *Pages/Courses* folder
- Run the app, and the Courses page will be available in browser. However, the Department column only have ID numbers, not department names.

## Display the Department name
- Update *Index.cshtml* and *Index.cshtml.cs*
- CourseID is shown as "Number" in the column header
- In RazorPage, `@Html.DisplayFor(modelItem => item.Department.Name)` will displays the Name property of the Department entity that's loaded into the Department navigation property