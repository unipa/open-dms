using FirmeRemoteLib.Implementation;
using FirmeRemoteLib.Interfaces;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenDMS.Startup;
using RemoteSign.BL;
using RemoteSignInfocert.Context;
using RemoteSignInfocert.Controllers;
using RemoteSignInfocert.DAO;
using RemoteSignInfocert.Interfaces;
using RemoteSignInfocert.Job;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

GlobalConfiguration.Configuration.UseMemoryStorage();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(logging => logging.AddConsole());
var config = builder.Configuration;
var seqUrl = config["seqUrl"];
builder.Logging.AddLogging("RemoteSign.API", seqUrl);


builder.WebHost.UseKestrel(conf =>
{
    conf.Limits.MaxRequestBufferSize = int.MaxValue;
    conf.Limits.MaxRequestLineSize = int.MaxValue;
    conf.Limits.MaxRequestBodySize = long.MaxValue;
});

builder.Services.AddDbContext<UserDbContext>(p => p.UseSqlite());
Console.WriteLine("Environment is Production: " + builder.Environment.IsProduction());
// Add services to the container.
builder.Services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions
                           .ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                });

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

ServicePointManager.Expect100Continue = true;
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                       | SecurityProtocolType.Tls11
                                       | SecurityProtocolType.Tls12
                                       | SecurityProtocolType.Tls13;


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DMS.RemoteSignInfocert.API",
        Description = "Servizio per la Sottoscrizione con Firma Remota",
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
  
});

//builder.Services.AddLogging(conf => conf.AddSerilog(Log.Logger));

builder.Services.AddTransient<IUserDAO, UserDAO>();
builder.Services.AddTransient<ISignRoomDAO, SignRoomDAO>();
builder.Services.AddTransient<SignService>();
builder.Services.AddTransient<DeliveryJob>();
builder.Services.AddTransient<SignController>();
if (!String.IsNullOrEmpty(config["Debug"]))
    builder.Services.AddTransient<IRemoteSign, DummySign>();
else
    builder.Services.AddTransient<IRemoteSign, InfocertUnipaSign>();

builder.Services.AddHangfire(hangfire => { });
builder.Services.AddHangfireServer();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseSwagger(c =>
{
    c.RouteTemplate = "/dms/RemoteSignInfocert/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/dms/RemoteSignInfocert/swagger/v1/swagger.json", "OpenDMS.RemoteSign.API V1.1");
    c.RoutePrefix = "dms/RemoteSignInfocert/swagger";
});
app.UsePathBase(config["PATH_BASE"]);

using (var scope = app.Services.CreateScope())
{
    var deliveryJob = scope.ServiceProvider.GetRequiredService<DeliveryJob>();
    RecurringJob.AddOrUpdate("CheckForDelivery", () => deliveryJob.CheckForDelivery(), Cron.MinuteInterval(20));
    RecurringJob.AddOrUpdate("CheckForExpiredSignRoom", () => deliveryJob.CheckForExpiredSignRoom(), Cron.Daily(1));

    try
    {
        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        if (context != null)
        {
            if (context.Database.GetPendingMigrations().Count() > 0)
            {
                context.Database.Migrate();
            }
        }
    }
    catch {
    }
}

// Configure the HTTP request pipeline.

app.UseExceptionHandler("/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/",
     dataTokens: new { pathBase = config["PATH_BASE"] });
try
{
    app.Run();
}
catch (Exception ex) { Console.WriteLine(ex.Message); }
