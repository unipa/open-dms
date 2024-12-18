using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenDMS.CustomPages;
using OpenDMS.CustomPages.Models;
using OpenDMS.Core;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.LuceneIndexer;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Infrastructure.Providers;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Database;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.Infrastructure.Services;
using OpenDMS.Startup;
using System.Net;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
//using System.Text.Json.Serialization;

namespace OpenDMS.TaskList.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLogging(logging => logging.AddConsole());
            var config = builder.Configuration;
            var seqUrl = config["seqUrl"];
            builder.Logging.AddLogging("TaskList.API", seqUrl);


            IdentityModelEventSource.ShowPII = true;

            builder.Services.AddMultiTenancy(builder.Configuration);
            builder.Services.AddRepositories();
            
            //builder.Services.AddScoped<IVirtualFileSystem, AbstractFileSystem>();
            //builder.Services.AddScoped<IVirtualFileSystemProvider, VirtualFileSystemProvider>();
            builder.Services.AddStorage();
            builder.Services.AddExternalServices(builder.Configuration);
            builder.Services.AddTransient<IMailboxRepository, MailboxRepository>();
            builder.Services.AddTransient<IMailboxService, MailboxService>();
            builder.Services.AddTransient<ISearchEngine, LuceneDocumentIndexer>();
            builder.Services.AddCoreServices();
            builder.Services.AddExternalIdentity(builder.Configuration, builder.Environment.IsProduction());

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            });
            builder.Services.AddRazorPages();
            //.AddJsonOptions(options =>
            //options.JsonSerializerOptions.ReferenceHandler =  ReferenceHandler.IgnoreCycles);

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OpenDMS.TaskList.API",
                    Description = "Servizio per l'amministrazione di un Tenant",
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
                   );

            var app = builder.Build();
            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/api/tasklist/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/tasklist/swagger/v1/swagger.json", "OpenDMS.Admin.API V1");
                c.RoutePrefix = "api/tasklist/swagger";
            });
            app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMultiTenancy();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=Index}/{id?}");


//            ApplicationService.Register(config["EndPoint:UIService"],
//                new ApplicationLink() { Id = "TaskList", Icon = "fa fa-bell", Label = "Attiivt�", Tooltip = "Attivit�", URL = "/dms/Tasks" }
//            ).GetAwaiter().GetResult();



            app.Run();
        }
    }
}