using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenDMS.Core;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DataTypes;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.LuceneIndexer;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Infrastructure.Providers;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Database;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.Infrastructure.Services.BusinessLogic;
using OpenDMS.Preservation.API;
using OpenDMS.Preservation.Core.Implementations;
using OpenDMS.Preservation.Core.Interfaces;
using OpenDMS.Startup;

using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddLogging(logging => logging.AddConsole());
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(Environment.CurrentDirectory, @"Log\OpenDMS.ConservazioneSostitutiva.log"), rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddMultiTenancy(builder.Configuration);
builder.Services.AddRepositories();


builder.Services.AddStorage();
builder.Services.AddExternalServices(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddExternalIdentity(builder.Configuration, builder.Environment.IsProduction(), false);
builder.Services.AddLucene();


//builder.Services.AddScoped<ISearchEngine, LuceneDocumentIndexer>();


//builder.Services.AddScoped<IDocumentService, DocumentService>();
//builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
//builder.Services.AddScoped<IDataTypeFactory, DataTypeFactory> ();



IdentityModelEventSource.ShowPII = true;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IPreservationWorker, PreservationWorker>();
builder.Services.AddHostedService<PreservationHostingService>();


var app = builder.Build();

app.UseSwagger(c =>
{
    c.RouteTemplate = "/api/preservation/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/preservation/swagger/v1/swagger.json", "OpenDMS.Admin.API V1");
    c.RoutePrefix = "api/preservation/swagger";
});


//DB INTERNO CONFIG
/*builder.Services.AddDbContext<ICSContext, SqliteCSDbContext>(option =>
  option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);*/
//DB INITIAL MIGRATION
/*using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<SqliteCSDbContext>();
    dataContext.Database.Migrate();
}*/
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
