using OpenDMS.TitulusIntegration.API.BL;
using OpenDMS.TitulusIntegration.API.BL.Interfacce;
using OpenDMS.Core;
using OpenDMS.Core.LuceneIndexer;
using OpenDMS.Startup;
using OpenDMS.Infrastructure.Database;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddLogging(logging => logging.AddConsole());
builder.Logging.AddLogging("TitulusAPI");

IdentityModelEventSource.ShowPII = true;

//Add services
builder.Services.AddMultiTenancy(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddStorage();
builder.Services.AddExternalServices(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddLucene();
builder.Services.AddExternalIdentity(builder.Configuration, builder.Environment.IsProduction());

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITitulusBL, TitulusBL>();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "OpenDMS.TitulusIntegration.API",
        Description = "Servizio per l'integrazione con Titulus",
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
app.UseSwagger();
app.UseSwaggerUI();
app.UseSwagger(c =>
{
    c.RouteTemplate = "/api/titulus/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/titulus/swagger/v1/swagger.json", "OpenDMS.Titulus.API V1");
    c.RoutePrefix = "api/titulus/swagger";
});
app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });
app.UseHttpsRedirection();
app.UseMultiTenancy();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
