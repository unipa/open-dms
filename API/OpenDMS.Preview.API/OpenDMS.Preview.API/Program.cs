using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenDMS.Core;
using OpenDMS.Core.LuceneIndexer;
using OpenDMS.Domain.Worker;
using OpenDMS.Infrastructure.Database;
using OpenDMS.Infrastructure.Services.Monitor;
using OpenDMS.Infrastructure.Services.Workers;
using OpenDMS.Startup;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace OpenDMS.DocumentManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLogging(logging => logging.AddConsole());
            var config = builder.Configuration;
            var seqUrl = config["seqUrl"];
            builder.Logging.AddLogging("Preview.API", seqUrl);


            IdentityModelEventSource.ShowPII = true;


            builder.Services.AddMultiTenancy(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddStorage();
            builder.Services.AddExternalServices(builder.Configuration);
            builder.Services.AddCoreServices();
            builder.Services.AddLucene();
            builder.Services.AddExternalIdentity(builder.Configuration, builder.Environment.IsProduction());

            builder.Services.AddSingleton<IMessageBusMonitor, PreviewQueueMonitor>();
            builder.Services.AddHostedService<PreviewGeneratorWorker>();



            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            });
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
                    Title = "DPM.Preview.API",
                    Description = "Servizio per la produzione delle anteprime dei documenti",
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
                       o => o.AddPolicy("admin", b => b.RequireClaim("roles", "user").RequireAuthenticatedUser())
                   );


            var app = builder.Build();
            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/api/preview/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/preview/swagger/v1/swagger.json", "OpenDMS.Admin.API V1");
                c.RoutePrefix = "api/preview/swagger";
            });
            app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });
            app.UseHttpsRedirection();
            app.UseMultiTenancy();
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