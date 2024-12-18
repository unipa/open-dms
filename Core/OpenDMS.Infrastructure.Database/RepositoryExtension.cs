using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Database.Builder;
using OpenDMS.Infrastructure.Database.Repositories;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data.Common;
using OpenDMS.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Infrastructure.Database
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("System.Data.SQLite", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("System.Data.Odbc", System.Data.Odbc.OdbcFactory.Instance);
            if (System.Data.OleDb.OleDbFactory.Instance != null) DbProviderFactories.RegisterFactory("System.Data.OleDb", System.Data.OleDb.OleDbFactory.Instance);
            DbProviderFactories.RegisterFactory("System.Data.Mysql", MySql.Data.MySqlClient.MySqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("npgsql", Npgsql.NpgsqlFactory.Instance);
            DbProviderFactories.RegisterFactory("System.Data.OracleClient", Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);
            DbProviderFactories.RegisterFactory("Sap.Data.Hana", Sap.Data.Hana.HanaFactory.Instance);


            //            services.AddTransient<IApplicationDbContextFactory>(s => new ApplicationDbContextFactory(() => { return tenant; }));
            services.AddTransient<IApplicationDbContextFactory, ApplicationDbContextFactory>();
            services.AddTransient<IAppSettingsRepository, AppSettingRepository>();
            services.AddTransient<ITranslationRepository, TranslationRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IACLRepository, ACLRepository>();
            services.AddTransient<IDataSourceProvider, DataSourceProvider>();
            services.AddTransient<ICustomFieldTypeRepository, CustomDataTypeRepository>();
            services.AddTransient<ILookupTableRepository, LookupTableRepository>();
            services.AddTransient<IDocumentTypeRepository, DocumentTypeRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<IHistoryRepository, HistoryRepository>();

            services.AddTransient<ISearchFilterRepository, SearchFilterRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<ICustomPagesRepository, CustomPagesRepository>();
            services.AddTransient<IUISettingsRepository, UISettingRepository>();
            services.AddTransient<IUserSettingsRepository, UserSettingRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOrganizationRepository, OrganizationRepository>();
            services.AddTransient<IUserGroupRepository, UserGroupRepository>();
            services.AddTransient<IMailServerRepository, MailServerRepository>();
            services.AddTransient<IMailEntryRepository, MailEntryRepository>();
            services.AddTransient<IMailboxRepository, MailboxRepository>();
            services.AddTransient<ITaskEndpointRepository, TaskEndpointRepository>();
            services.AddTransient<IProcessInstanceRepository, ProcessInstanceRepository>();

            services.AddTransient<ISqlBuilder, SqlBuilder>();
            services.AddTransient<IQueryBuilder, QueryBuilder>();
            //services.AddScoped<IDocumentSearchRepository, DocumentSearchRepository>();
            //services.AddTransient<IViewManagerRepository, ViewManagerRepository>();
            //services.AddTransient<IProcessDataRepository, ProcessDataRepository>();

            services.AddTransient<IDistributedLocker, DistributedLocker>();
            return services;
        }

        public static IApplicationBuilder UpdateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContextFactory>().GetDbContext();
                try
                {
                    var sec = Random.Shared.Next(5);
                    Thread.Sleep(sec * 1000);
                    context.Database.Migrate();
                    //if (context != null)
                    //{
                    //    context.Database.OpenConnection();
                    //    context.Database.CloseConnection();
                    //}
                }
                catch
                {
                    //context.Database.Migrate();
                }
            }
            //using var scope = app.ApplicationServices.CreateScope();
            //using var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContextFactory>().GetDbContext();
            //dbContext.Database.Migrate();
            return app;
        }
    }
}
