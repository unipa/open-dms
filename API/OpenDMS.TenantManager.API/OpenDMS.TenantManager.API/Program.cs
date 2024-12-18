using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Infrastructure.Encrypters;
using OpenDMS.Domain.Infrastructure.Providers;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.Startup;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace OpenDMS.TenantManager.API;

/// <summary>
///
/// {
///   "id": "localhost",
///   "description": "tenant 1",
///   "connectionString": "",
///   "databaseConnectionStrategy": 0,
///   "provider" :"sqlserver",
///   "offline": false
/// }
/// </summary>


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;
        builder.Services.AddLogging(logging => logging.AddConsole());
        var seqUrl = config["seqUrl"];
        builder.Logging.AddLogging("UserManager.API", seqUrl);

        IdentityModelEventSource.ShowPII = true;
        builder.Services.AddMultiTenancy(builder.Configuration);
        builder.Services.AddStorage();
        //builder.Services.AddScoped<IApplicationDbContextFactory, ApplicationDbContextFactory>();
        builder.Services.AddTransient<IFileEncryptor, FastFileEncryptor>();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        //builder.Services.Configure<CookiePolicyOptions>(options =>
        //{
        //    options.CheckConsentNeeded = context => true;
        //    options.MinimumSameSitePolicy = SameSiteMode.None;
        //});
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = $"{config["Keycloak:auth-server-url"]}realms/{config["Keycloak:realm"]}";
            options.RequireHttpsMetadata = builder.Environment.IsProduction();
            if (!builder.Environment.IsProduction())
                options.BackchannelHttpHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (m, c, s, o) => { return true; },
                    UseDefaultCredentials = true,
                };
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "preferred_username",
                RoleClaimType = "roles"
            };

            options.Events = new JwtBearerEvents()
            {
            };
            options.Audience = "account";
            options.MetadataAddress = $"{config["Keycloak:auth-server-url"]}realms/{config["Keycloak:realm"]}/.well-known/openid-configuration";
        })
        .AddOpenIdConnect(options =>
        {
            options.Authority = $"{config["Keycloak:auth-server-url"]}realms/{config["Keycloak:realm"]}";
            options.ClientId = config["Keycloak:resource"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "preferred_username",
                RoleClaimType = "roles"
            };
            options.RequireHttpsMetadata = builder.Environment.IsProduction();
            if (!builder.Environment.IsProduction())
                options.BackchannelHttpHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (m, c, s, o) => { return true; },
                    UseDefaultCredentials = true,
                };
            options.SaveTokens = true;
            options.ClientSecret = config["Keycloak:Credentials:secret"];
            options.GetClaimsFromUserInfoEndpoint = true;
            options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
        });

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
        });
        builder.Services.AddRazorPages();
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
                Title = "OpenDMS.TenantManager.API",
                Description = "Servizio per la gestione dei Tenant",
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

        var app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "/api/tenantmanager/swagger/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/api/tenantmanager/swagger/v1/swagger.json", "OpenDMS.Admin.API V1");
            c.RoutePrefix = "api/tenantmanager/swagger";
        });
        app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });
        app.UsePathBase(config["PATH_BASE"]);
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseMultiTenancy();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=home}/{action=Index}/{id?}");


        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<IMasterTenantDbContextFactory>().GetDbContext();
        try
        {
            dbContext.Database.Migrate();
            app.Run();
        }
        catch { };
    }
}