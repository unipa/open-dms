dotnet ef migrations remove --project  ..\..\Core\OpenDMS.Infrastructure.Database  --context MySqlDbContext 
dotnet ef migrations remove --project  ..\..\Core\OpenDMS.Infrastructure.Database  --context SqliteDbContext 
dotnet ef migrations remove --project  ..\..\Core\OpenDMS.Infrastructure.Database  --context SqlServerDbContext 
dotnet ef migrations remove --project  ..\..\Core\OpenDMS.Infrastructure.Database  --context OracleDbContext 
