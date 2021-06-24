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