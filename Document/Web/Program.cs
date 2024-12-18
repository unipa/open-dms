using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenDMS.Core;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.LuceneIndexer;
using OpenDMS.CustomPages;
using OpenDMS.CustomPages.Implementation;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Database;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.Startup;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Web.BL;
using Web.BL.Interface;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            //builder.Services.AddLogging(logging => logging.AddConsole());
            var seqUrl = config["seqUrl"];
            builder.Logging.AddLogging("FrontEnd.API", seqUrl);


            IdentityModelEventSource.ShowPII = true;

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                long fileSizeLimit = (long)4 * 1024 * 1024 * 1024; //4gb
                serverOptions.Limits.MaxRequestBodySize = fileSizeLimit;
            });

            // Add services to the container.
            builder.Services.AddMultiTenancy(builder.Configuration);
            builder.Services.AddStorage();
            builder.Services.AddRepositories();
            builder.Services.AddTransient<IAdminLeftMenuBL, AdminLeftMenuBL>();
            builder.Services.AddTransient<IBancheDatiBL, BancheDatiBL>();
            builder.Services.AddTransient<IPermessiBL, PermessiBL>();
            builder.Services.AddTransient<ITabelleInterneBL, TabelleInterneBL>();
            builder.Services.AddTransient<ITemplateNotificheBL, TemplateNotificheBL>();
            builder.Services.AddTransient<IMailServerBL, MailServerBL>();
            builder.Services.AddTransient<IDocProcessesBL, DocProcessesBL>();
            builder.Services.AddTransient<IOrganigrammaBL, OrganigrammaBL>();
            builder.Services.AddTransient<ICustomizeLeftPanelBL, CustomizeLeftPanelBL>();
            builder.Services.AddTransient<ICustomizeBL, CustomizeBL>();
            builder.Services.AddTransient<IUserContactDigitalAddressBL, UserContactDigitalAddressBL>();
            builder.Services.AddTransient<ICustomPageProvider, CustomPageProvider>();

            builder.Services.AddExternalServices(config);
            builder.Services.AddCoreServices();
            builder.Services.AddExternalIdentity(builder.Configuration, builder.Environment.IsProduction(), false);
            builder.Services.AddLucene();
            builder.Services.AddServiceWorkers(builder.Configuration);

            builder.Services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            });
            builder.Services.AddRazorPages();
            builder.Services.AddAntiforgery(x =>
            {
                x.SuppressXFrameOptionsHeader = true;
            });

            builder.Services.AddAuthorization(
                        o => o.AddPolicy("admin", b => b.RequireClaim("roles", "admin").RequireAuthenticatedUser())
            );

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OpenDMS.Internal.API",
                    Description = "API Interne",
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


            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/internalapi/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/internalapi/swagger/v1/swagger.json", "OpenDMS.API V1");
                c.RoutePrefix = "internalapi/swagger";
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });

            //app.UseStatusCodePagesWithRedirects("/Errors/{0}");
            app.UseExceptionHandler("/Error");
            //app.UseStatusCodePagesWithReExecute("/Errors");

            if (!app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Errors");
                //app.UseStatusCodePagesWithReExecute("/Errors");
                app.UseHsts();
            }
         //   else
         //       app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            app.UseMultiTenancy();
            app.UseAuthorization();

            app.MapControllers();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=Index}/{id?}");

            app.UpdateDatabase();
            await app.AddTemplates();
            app.Run();
        }
    }
}