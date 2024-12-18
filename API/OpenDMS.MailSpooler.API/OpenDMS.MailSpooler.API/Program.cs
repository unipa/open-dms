using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenDMS.Core;
using OpenDMS.Core.LuceneIndexer;
using OpenDMS.Domain.Worker;
using OpenDMS.Infrastructure.Database;
using OpenDMS.Infrastructure.Services.Monitor;
using OpenDMS.MailSpooler.API.Service;
using OpenDMS.MailSpooler.Core;
using OpenDMS.MailSpooler.Core.Interfaces;
using OpenDMS.MailSpooler.Service;
using OpenDMS.Startup;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(logging => logging.AddConsole());
var config = builder.Configuration;
var seqUrl = config["seqUrl"];
builder.Logging.AddLogging("MailSpooler.API", seqUrl);
IdentityModelEventSource.ShowPII = true;
builder.Services.AddMultiTenancy(builder.Configuration);
builder.Services.AddRepositories();

builder.Services.AddStorage();
builder.Services.AddExternalServices(config);
builder.Services.AddCoreServices();
builder.Services.AddExternalIdentity(builder.Configuration, builder.Environment.IsProduction(), false);
builder.Services.AddLucene();
builder.Services.AddServiceWorkers(builder.Configuration);


//builder.Services.AddScoped<IMessaging<string>, RabbitMQMessageBus<string>>();
//builder.Services.AddScoped<ISearchEngine, LuceneDocumentIndexer>();
builder.Services.AddTransient<IAuthenticatorFactory, AuthenticatorFactory>();

builder.Services.AddHostedService<MailHostingService>();
builder.Services.AddHostedService<SenderHostingService>();
builder.Services.AddHostedService<ReceiverHostingService>();

builder.Services.AddSingleton<IMessageBusMonitor, MailMessageQueueMonitor>();

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
        Title = "OpenDMS.MailSpooler.API",
        Description = "Servizio per lo scodamento dei messaggi di posta elettronica",
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

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();
app.UseSwagger(c =>
{
    c.RouteTemplate = "/api/mailspooler/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/mailspooler/swagger/v1/swagger.json", "OpenDMS.Admin.API V1");
    c.RoutePrefix = "api/mailspooler/swagger";
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
app.Run();