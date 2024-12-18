dotnet ef migrations add %1 --project  ..\..\Core\OpenDMS.Infrastructure.Database --output-dir Migrations/MySql --context MySqlDbContext 
dotnet ef migrations add %1 --project  ..\..\Core\OpenDMS.Infrastructure.Database --output-dir Migrations/Sqlite --context SqliteDbContext 
dotnet ef migrations add %1 --project  ..\..\Core\OpenDMS.Infrastructure.Database --output-dir Migrations/SqlServer --context SqlServerDbContext 
dotnet ef migrations add %1 --project  ..\..\Core\OpenDMS.Infrastructure.Database --output-dir Migrations/Oracle --context OracleDbContext 
