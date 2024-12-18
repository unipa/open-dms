dotnet ef migrations add %1 --output-dir Migrations/MySql --context MySqlDbContext 
dotnet ef migrations add %1 --output-dir Migrations/Sqlite --context SqliteDbContext 
dotnet ef migrations add %1 --output-dir Migrations/SqlServer --context SqlServerDbContext 
dotnet ef migrations add %1 --output-dir Migrations/Oracle --context OracleDbContext 



//dotnet ef migrations add InitialCreate --project Microsoft.AspNetCore.Identity  --output-dir Migrations/MySql --context MySqlApplicationDbContext 

