dotnet ef migrations add %1 --project ../../../Core/OpenDMS.MultiTenancy --output-dir Migrations/MySql --context MySqlTenantRegistryDbContext 
dotnet ef migrations add %1 --project ../../../Core/OpenDMS.MultiTenancy --output-dir Migrations/Sqlite --context SqliteTenantRegistryDbContext
dotnet ef migrations add %1 --project ../../../Core/OpenDMS.MultiTenancy --output-dir Migrations/SqlServer --context SqlServerTenantRegistryDbContext
dotnet ef migrations add %1 --project ../../../Core/OpenDMS.MultiTenancy --output-dir Migrations/Oracle --context OracleTenantRegistryDbContext


