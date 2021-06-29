
## Migrations for SQLite
- Install Entity Framework CLI `dotnet tool install --global dotnet-ef`
- Drop the SQLite database **"CU.db"** `dotnet ef database drop --force`
- Create initial migration `dotnet ef migrations add InitialCreate`
- Re-create the SQLite database **"CU.db"** `dotnet ef database update`
