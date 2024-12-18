using A3Synch.BL;
using A3Synch.Context;
using A3Synch.DAO;
using A3Synch.Interfacce;
using A3Synch.Utility;
using Elmi.Core.FileConverters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenDMS.Core;
using OpenDMS.Core.LuceneIndexer;
using OpenDMS.Infrastructure.Database;
using OpenDMS.Infrastructure.Services.Workers;
using OpenDMS.Startup;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace OpenDMS.A3Synch.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddLogging(logging => logging.AddConsole());
            var config = builder.Configuration;
            var seqUrl = config["seqUrl"];
            builder.Logging.AddLogging("A3Synch.API", seqUrl);
            IdentityModelEventSource.ShowPII = true;
            builder.Services.AddMultiTenancy(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddStorage();
            builder.Services.AddExternalServices(builder.Configuration);
            

            builder.Services.AddCoreServices();
            builder.Services.AddExternalIdentity(builder.Configuration, false);
            builder.Services.AddScoped<IFileConvertFactory, FileConvertFactory>();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            }); 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHostedService<A3SynchWorker>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OpenDMS.A3Synch.API",
                    Description = "Servizio per la sincronizzazione dell'Organigramma A3 con i DB di DMS",
                    TermsOfService = new Uri("https://www.elmisrl.eu/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Contacts",
                        Url = new Uri("https://www.elmisrl.eu/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://www.elmisrl.eu/license")
                    }
                }
                );
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Auth",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OpenIdConnect,
                    OpenIdConnectUrl = new Uri(config["Keycloak:auth-server-url"] + "realms/" + config["Keycloak:realm"] + "/.well-known/openid-configuration"),
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, Array.Empty<string>()}
            });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddAuthorization(
                       o => o.AddPolicy("admin", b => b.RequireClaim("roles", "admin").RequireAuthenticatedUser())
                   );

            var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider").ToLower();


            // Aggiungi il DbContext corretto in base alla configurazione
            switch (databaseProvider)
            {
                case "mysql":
                    builder.Services.AddDbContext<MySqlContext>();
                    builder.Services.AddScoped<IContext>(provider => provider.GetService<MySqlContext>());
                    break;
                case "mssql":
                case "sqlserver":
                    builder.Services.AddDbContext<SqlServerContext>();
                    builder.Services.AddScoped<IContext>(provider => provider.GetService<SqlServerContext>());
                    break;
                case "oracle":
                    builder.Services.AddDbContext<OracleContext>();
                    builder.Services.AddScoped<IContext>(provider => provider.GetService<OracleContext>());
                    break;
                default:
                    throw new InvalidOperationException($"Il Provider '{databaseProvider}' non è supportato.");
            }



            
            builder.Services.AddScoped<IKeycloakBL, KeycloakBL>();
            builder.Services.AddScoped<IUtils, Utils>();
            builder.Services.AddScoped<IUserGroupRolesBL, UserGroupRolesBL>();
            builder.Services.AddScoped<IUserGroupsBL, UserGroupsBL>();
            builder.Services.AddScoped<IRolesBL, RolesBL>();
            builder.Services.AddScoped<IUserGroupsDAO, UserGroupsDAO>();
            builder.Services.AddScoped<IUsersDAO, UsersDAO>();
            builder.Services.AddScoped<IUsersBL, UsersBL>();
            builder.Services.AddScoped<IUserGroupRolesDAO, UserGroupRolesDAO>();
            builder.Services.AddScoped<IRolesDAO, RolesDAO>();
			builder.Services.AddScoped<IOrganizationNodesBL, OrganizationNodesBL>();
			builder.Services.AddScoped<IOrganizationNodesDAO, OrganizationNodesDAO>();
            builder.Services.AddScoped<IUserGroupsDAO, UserGroupsDAO>();
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddLucene();

            var app = builder.Build();
            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/api/a3synch/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/a3synch/swagger/v1/swagger.json", "OpenDMS.Admin.API V1");
                c.RoutePrefix = "api/a3synch/swagger";
            });
            app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
