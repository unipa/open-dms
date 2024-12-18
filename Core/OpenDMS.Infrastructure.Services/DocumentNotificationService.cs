
using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace OpenDMS.Infrastructure.Services
{
    public class DocumentNotificationService : IDocumentNotificationService
    {
        private readonly IMessaging<string> _messaging;
        private readonly ILogger<DocumentNotificationService> logger;
        private readonly IServiceProvider serviceProvider;

        private readonly IConfiguration _config;
        private readonly IAppSettingsRepository appSettings;
        private readonly string queueName;

        public DocumentNotificationService(IMessaging<string> messaging, 
            ILogger<DocumentNotificationService> logger, 
            IServiceProvider serviceProvider,
            IConfiguration configuration, 
            IAppSettingsRepository appSettings)
        {
            _messaging = messaging;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            _config = configuration;
            this.appSettings = appSettings;
            queueName = _config.GetSection(StaticConfiguration.CONST_NOTIFICATIONSERVICE_QUEUE)?.Value;
        }

        public async Task Notify(DocumentNotification notification, UserProfile userInfo)
        {
            string msg = System.Text.Json.JsonSerializer.Serialize(notification);
            if (String.IsNullOrEmpty(queueName))
            {
                logger.LogWarning("SendNotification", "<EmptyQueue>: "+msg);
                //using (var scope = serviceProvider.CreateScope())
                //{
                IUserTaskService taskService = serviceProvider.GetRequiredService<IUserTaskService>();
                IUserService userService = serviceProvider.GetRequiredService<IUserService>();
                CreateNewTask T = new CreateNewTask()
                {
                    Attachments = notification.Documents,
                    MessageTemplate = notification.TemplateId,
                    CompanyId = notification.CompanyId,
                    AssignToAllUsers = notification.AssignToAllUsers,
                    Description = notification.Message,
                    NotifyCC = notification.CC.Select(n => ((int)n.ProfileType).ToString() + n.ProfileId).ToList(),
                    NotifyTo = notification.To.Select(n => ((int)n.ProfileType).ToString() + n.ProfileId).ToList(),
                    Sender = notification.Sender,
                    //Sender = "0" + notification.Sender,
                    TaskType = notification.RequestType == Domain.Enumerators.ActionRequestType.Generic 
                    ? Domain.Enumerators.TaskType.Activity
                    : notification.RequestType == Domain.Enumerators.ActionRequestType.None
                        ? Domain.Enumerators.TaskType.Message
                        : notification.RequestType == Domain.Enumerators.ActionRequestType.Warning
                            ? Domain.Enumerators.TaskType.Warning
                            : notification.RequestType == Domain.Enumerators.ActionRequestType.Error
                                ? Domain.Enumerators.TaskType.Error
                                : Domain.Enumerators.TaskType.Event,
                    Title = notification.Title
                };
                if (T.TaskType == Domain.Enumerators.TaskType.Event)
                {
                    switch (notification.RequestType)
                    {
                        case Domain.Enumerators.ActionRequestType.View_Document:
                            T.EventId = EventType.View;
                            break;
                        case Domain.Enumerators.ActionRequestType.Approve_Document:
                            T.EventId = EventType.Approval;
                            break;
                        case Domain.Enumerators.ActionRequestType.Check_Document:
                            T.EventId = EventType.AddCheckSign;
                            break;
                        case Domain.Enumerators.ActionRequestType.Sign_Document:
                            T.EventId = EventType.AddSignatureField;
                            break;
                        case Domain.Enumerators.ActionRequestType.FEA_Document:
                            T.EventId = EventType.AddBiometricalSignature;
                            break;
                        case Domain.Enumerators.ActionRequestType.DigitalSign_Document:
                            T.EventId = EventType.AddDigitalSignature;
                            break;
                        case Domain.Enumerators.ActionRequestType.Folder_Document:
                            T.EventId = EventType.AddToFolder;
                            break;
                        case Domain.Enumerators.ActionRequestType.Protocol_Document:
                            T.EventId = EventType.Protocol;
                            break;
                        default:
                            T.EventId = "";
                            break;
                    }
                }
                await taskService.CreateTask(T, userInfo);// await userService.GetUserProfile(notification.Sender));

            }
            else
            {
                try
                {
                    _messaging.PushMessage(msg, queueName);
                    logger.LogDebug("SendNotification", queueName + ":" + msg);
                }
                catch (Exception Ex)
                {
                    logger.LogError(Ex, "SendNotification", queueName + ":" + msg);
                    throw;
                }
            }
            await Task.CompletedTask;
        }

        public async Task NotifyException( UserProfile userInfo, Exception ex, Document d = null)
        {
            var Template = await appSettings.Get(NotificationConstants.CONST_TEMPLATE_NOTIFY_EXCEPTION);
            if (string.IsNullOrEmpty(Template))
                Template = "{Message}\n\nStack Trace: {StackTrace}";
            DocumentNotification error_notification = new DocumentNotification();
            error_notification.Sender = userInfo.userId;
            error_notification.CompanyId = d.CompanyId;
            error_notification.NotificationDate = DateTime.Now;
            error_notification.Message =Template.Replace("{Message}",ex.Message).Replace("{StackTrace}", ex.StackTrace);
            error_notification.Documents = new List<int> { d.Id };
            error_notification.RequestType = Domain.Enumerators.ActionRequestType.Error;

            error_notification.Exception = true;

            var listaUtentiDestinatari = await appSettings.Get(NotificationConstants.CONST_NOTIFICATION_ERROR_USERS_RECIPIENTS);
            foreach (var utente in listaUtentiDestinatari.Split(","))
                error_notification.To.Add(new ProfileInfo { ProfileId = utente, ProfileType = Domain.Enumerators.ProfileType.User });
            var listaGruppiDestinatari = await appSettings.Get(NotificationConstants.CONST_NOTIFICATION_ERROR_GROUPS_RECIPIENTS);
            foreach (var utente in listaGruppiDestinatari.Split(","))
                error_notification.To.Add(new ProfileInfo { ProfileId = utente, ProfileType = Domain.Enumerators.ProfileType.Group });
            var listaRuoliDestinatari = await appSettings.Get(NotificationConstants.CONST_NOTIFICATION_ERROR_ROLES_RECIPIENTS);
            foreach (var utente in listaRuoliDestinatari.Split(","))
                error_notification.To.Add(new ProfileInfo { ProfileId = utente, ProfileType = Domain.Enumerators.ProfileType.Role });

            await Notify(error_notification, userInfo);
        }



    }
}