using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenDMS.Core;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.LuceneIndexer;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Services;
using OpenDMS.Domain.Worker;
using OpenDMS.Infrastructure.Database;
using OpenDMS.Infrastructure.Services.BusinessLogic;
using OpenDMS.Infrastructure.Services.Monitor;
using OpenDMS.MailSpooler.Core;
using OpenDMS.MailSpooler.Core.Interfaces;
using OpenDMS.Startup;
using OpenDMS.Workflow.API.BusinessLogic;
using OpenDMS.Workflow.API.BusinessLogic.CustomTask;
using OpenDMS.Workflow.API.BusinessLogic.MaximoBL;
using OpenDMS.Workflow.API.BusinessLogic.MaximoBL.Interfacce;
using OpenDMS.Workflow.API.BusinessLogic.TitulusBL;
using OpenDMS.Workflow.API.BusinessLogic.TitulusBL.Interfacce;
using OpenDMS.Workflow.API.Service;
using OpenDMS.Workflow.API.Workers.Database;
using OpenDMS.Workflow.API.Workers.Documents;
using OpenDMS.Workflow.API.Workers.Maximo;
using OpenDMS.Workflow.API.Workers.Titulus;
using OpenDMS.Workflow.API.Workers.UserTasks;
using System.Net;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace OpenDMS.Workflow.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddLogging(logging => logging.AddConsole());
            var config = builder.Configuration;
            var seqUrl = config["seqUrl"];
            builder.Logging.AddLogging("Workflow.API", seqUrl);

            IdentityModelEventSource.ShowPII = true;

            builder.Services.AddMultiTenancy(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddStorage();

            builder.Services.AddExternalServices(builder.Configuration);
            builder.Services.AddCoreServices();
            builder.Services.AddExternalIdentity(builder.Configuration, builder.Environment.IsProduction());
            builder.Services.AddLucene();

            //builder.Services.AddServiceWorkers(builder.Configuration);

            builder.Services.AddSingleton<IMessageBusMonitor, ProcessQueueMonitor>();
            builder.Services.AddHostedService<ProcessHostingService>();


            builder.Services.AddTransient<IAuthenticatorFactory, AuthenticatorFactory>();

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
                    Title = "OpenDMS.Workflow.API",
                    Description = "Servizio per l'esecuzione dei workflow",
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

            //builder.Services.AddAuthorization();
            builder.Services.AddTransient<IBPMWorkerRegistry, ZeeBeWorkerRegistry>();
            builder.Services.AddTransient<IWorkflowEngine, ZeeBeWorkflowEngine>();
            builder.Services.AddTransient<IDocumentWorkflowEngine, DocumentWorkflowEngine>();

            //Depenjency Injection BL
            builder.Services.AddTransient<ITitulusBL, TitulusBL>();
            builder.Services.AddTransient<IMaximoSRBL, MaximoSRBL>();

            // TASKLIST WORKER
            builder.Services.AddTransient<ICustomTask, CompleteTaskWorker>();
            builder.Services.AddTransient<ICustomTask, RequestForEventTaskWorker>();
            builder.Services.AddTransient<ICustomTask, RequestForApprovalWorker>();
            builder.Services.AddTransient<ICustomTask, FormWorker>();
            builder.Services.AddTransient<ICustomTask, UserTask>();
            builder.Services.AddTransient<ICustomTask, UserMessageWorker>();

            builder.Services.AddTransient<ICustomTask, SqlWorker>();
            builder.Services.AddTransient<ICustomTask, SqlReaderWorker>();

            // DOCUMENT WORKER
            builder.Services.AddTransient<ICustomTask, AddAttachmentWorker>();
            builder.Services.AddTransient<ICustomTask, AddContentWorker>();
            builder.Services.AddTransient<ICustomTask, AddToFolderWorker>();
            builder.Services.AddTransient<ICustomTask, ChangeStatusWorker>();
            builder.Services.AddTransient<ICustomTask, CreateDocumentWorker>();
            builder.Services.AddTransient<ICustomTask, FillTemplateWorker>();
            builder.Services.AddTransient<ICustomTask, FindDocumentsWorker>();
            builder.Services.AddTransient<ICustomTask, GetDocumentWorker>();
            builder.Services.AddTransient<ICustomTask, SetPermissionWorker>();
            builder.Services.AddTransient<ICustomTask, GetDocumentContentWorker>();
            builder.Services.AddTransient<ICustomTask, FileConverterWorker>();
            builder.Services.AddTransient<ICustomTask, GetOrganizationRoleWorker> ();
            builder.Services.AddTransient<ICustomTask, SetDocumentFieldWorker>();
            builder.Services.AddTransient<ICustomTask, SendMail>();

            //MAXIMO WORKER
            builder.Services.AddTransient<ICustomTask, OpenSRWorker>();

            //KPI WORKER
            builder.Services.AddTransient<ICustomTask, StartTimerWorker>();
            builder.Services.AddTransient<ICustomTask, StopTimerWorker>();

            //TITULUS WORKER
            builder.Services.AddTransient<ICustomTask, CreateDocumentInDMSWorker>();
            builder.Services.AddTransient<ICustomTask, GetDocumentFromTitulusWorker>();
            builder.Services.AddTransient<ICustomTask, AddFilesToDocumentInDmsWorker>();
            builder.Services.AddTransient<ICustomTask, CreateInboundProtocolInTitulusWorker>();
            builder.Services.AddTransient<ICustomTask, CreateOutboundProtocolInTitulusWorker>();
            builder.Services.AddTransient<ICustomTask, CreateInternalProtocolInTitulusWorker>();


            builder.Services.AddTransient<ICustomTaskProvider, CustomTaskProvider>();
            builder.Services.AddTransient<IJobworkerStarter, JobworkerStarter>();
            builder.Services.AddTransient<ISearchEngine, LuceneDocumentIndexer>();

            var app = builder.Build();
            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/api/workflow/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/workflow/swagger/v1/swagger.json", "OpenDMS.Workflow.API V1.1");
                c.RoutePrefix = "api/workflow/swagger";
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
            var scope = app.Services.CreateScope();
            var starter = scope.ServiceProvider.GetService<IJobworkerStarter>();
            if (starter != null)
                starter.LoadTasks();
            var PaletteUpdater = scope.ServiceProvider.GetService<ICustomTaskProvider>();
            if (PaletteUpdater != null)
                PaletteUpdater.ImportPalette(scope.ServiceProvider);
            app.Run();
        }
    }
}