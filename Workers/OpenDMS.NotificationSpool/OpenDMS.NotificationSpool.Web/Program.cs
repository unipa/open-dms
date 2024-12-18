using MessageBus.Interface;
using MessageBus.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Services;
using OpenDMS.NotificationSpool.Core.Implementations;
using OpenDMS.NotificationSpool.Core.Interfaces;
using OpenDMS.NotificationSpool.Web.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(logging => logging.AddConsole());
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(Environment.CurrentDirectory, @"Log\OpenDMS.Notification.log"), rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}"
    )
    .CreateLogger();

// Register Serilog
builder.Logging.AddSerilog(logger);
IdentityModelEventSource.ShowPII = true;

builder.Services.AddDbContext<INotificationContext, SqliteTaskDbContext>(option => 
  option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Singleton);

builder.Services.AddTransient<IMessaging<string>, RabbitMQMessageBus<string>>();
builder.Services.AddTransient<INotificationWorker, NotificationWorker>();
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddTransient<IExtAPIManager, ExtAPIManager>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddHostedService<NotificationHostingService>();

var app = builder.Build();

//DB INITIAL MIGRATION
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<SqliteTaskDbContext>();
    dataContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

