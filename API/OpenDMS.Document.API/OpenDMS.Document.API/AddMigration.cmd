dotnet ef migrations add InitialCreate --project ../../../Core/OpenDMS.Infrastructure.Database --output-dir ../../../Core/OpenDMS.Infrastructure.Database/Migrations/MySql --context MySqlDbContext 
dotnet ef migrations add %1 --project ../../../Core/OpenDMS.Infrastructure.Database --output-dir ../../../Core/OpenDMS.Infrastructure.Database/Migrations/Sqlite --context SqliteDbContext
dotnet ef migrations add %1 --project ../../../Core/OpenDMS.Infrastructure.Database --output-dir ../../../Core/OpenDMS.Infrastructure.Database/Migrations/SqlServer --context SqlServerDbContext
dotnet ef migrations add %1 --project ../../../Core/OpenDMS.Infrastructure.Database --output-dir ../../../Core/OpenDMS.Infrastructure.Database/Migrations/Oracle --context OracleDbContext
