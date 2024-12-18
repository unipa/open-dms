using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenDMS.Core;
using OpenDMS.Infrastructure.Database;
using OpenDMS.Startup;
using System.Reflection;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.Domain.Repositories;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.MaximoSR.API.Utility.Interfacce;
using OpenDMS.MaximoSR.API.Utility;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using OpenDMS.MaximoSR.API.BL;
using OpenDMS.MaximoSR.API.BL.Interfacce;
using OpenDMS.Domain.Services;
using OpenDMS.Core.LuceneIndexer;
using Elmi.Core.FileConverters;

namespace OpenDMS.MaximoSR.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLogging(logging => logging.AddConsole());

            builder.Logging.AddLogging("UserManager.API");
            IdentityModelEventSource.ShowPII = true;
            builder.Services.AddMultiTenancy(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddStorage();
            builder.Services.AddExternalServices(builder.Configuration);
            builder.Services.AddScoped<IMailServerRepository, MailServerRepository>();
            builder.Services.AddScoped<IMailboxRepository, MailboxRepository>();
            builder.Services.AddScoped<IMailboxService, MailboxService>();
            builder.Services.AddCoreServices();
            builder.Services.AddExternalIdentity(builder.Configuration, builder.Environment.IsProduction());
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
            var config = builder.Configuration;
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddScoped<IMaximoSRBL, MaximoSRBL>();
            builder.Services.AddScoped<IZeebeHandler, ZeebeHandler>();
            builder.Services.AddScoped<ISearchEngine, LuceneDocumentIndexer>();
            builder.Services.AddScoped<IFileConvertFactory, FileConvertFactory>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OpenDMS.MaximoSR.API",
                    Description = "Servizio per l'apertura di un Ticket su Maximo",
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
                       o => o.AddPolicy("user", b => b.RequireClaim("roles", "user").RequireAuthenticatedUser())
                   );

            var app = builder.Build();
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/api/maximosr/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/maximosr/swagger/v1/swagger.json", "OpenDMS.Admin.API V1");
                c.RoutePrefix = "api/maximosr/swagger";
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