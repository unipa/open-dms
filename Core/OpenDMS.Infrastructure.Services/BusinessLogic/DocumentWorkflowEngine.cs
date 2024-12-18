using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;

namespace OpenDMS.Infrastructure.Services.BusinessLogic
{
    public class DocumentWorkflowEngine : IDocumentWorkflowEngine
    {
        private readonly ILogger<DocumentWorkflowEngine> logger;
        private readonly IDocumentTypeService typeService;
        private readonly IHistoryRepository historyRepository;
        private readonly IWorkflowEngine workflow;

        public DocumentWorkflowEngine(ILogger<DocumentWorkflowEngine> logger,
            IDocumentTypeService typeService,
            IHistoryRepository historyRepository,
            IWorkflowEngine workflow)
        {
            this.logger = logger;
            this.typeService = typeService;
            this.historyRepository = historyRepository;
            this.workflow = workflow;
        }


        public async Task HandleMessage(IEvent AppEvent)
        {
            try
            {
                string variables = System.Text.Json.JsonSerializer.Serialize(AppEvent.Variables);
                if (AppEvent.Variables.ContainsKey("Document"))
                {
                    var doc = (DocumentInfo)AppEvent.Get<DocumentInfo>("Document");
                    // Solo per i documenti attivi
                    //if (doc.DocumentStatus == Domain.Enumerators.DocumentStatus.Active)
                    //{
                    // Ignoro l'evento di visualizzazione
                    if (AppEvent.EventName != EventType.View)
                    {
                        // Recupero il documento che contiene la definizione di processo
                        var workflowDiagram = await typeService.GetWorkflow(doc.DocumentType.Id, AppEvent.EventName);
                        if (workflowDiagram != null)
                        {
                            logger.LogDebug("Trovato processo per TipoDocumento=" + workflowDiagram.DocumentTypeId + ", Evento=" + workflowDiagram.EventName);
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    var key = workflowDiagram.ProcessKey;
                                    var ProcessInstanceKey = await this.workflow.StartProcess(key, doc, AppEvent.UserInfo, AppEvent.EventName, variables);
                                    logger.LogDebug($"Avvio processo '{key}' - Istanza ID: {ProcessInstanceKey}");
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(ex, $"Avvio processo '{workflowDiagram.ProcessKey}'");
                                }
                            });
                        }
                        // Se il documento proviene da form e lo sto aggiornando, aggiorno le variabili dei processi attivi
                        if (AppEvent.EventName == EventType.Update && !String.IsNullOrEmpty(doc.DocumentType.CreationFormKey) && !String.IsNullOrEmpty(variables) && variables != "{}")
                        {
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    await this.workflow.UpdateProcessVariables(doc, AppEvent.UserInfo, "Document.AddVersion", variables);
                                    await this.workflow.UpdateProcessVariables(doc, AppEvent.UserInfo, "Document.AddRevision", variables);
                                }
                                catch (Exception ex)
                                {

                                    logger.LogError(ex, "Aggiornamento Variabili per evento '" + AppEvent.EventName + "' su documento Id." + doc.Id);
                                }
                            });

                        };

                        if (AppEvent.EventName != EventType.Creation
                            || AppEvent.EventName != EventType.Update
                            || AppEvent.EventName != EventType.AddAttach
                            || AppEvent.EventName != EventType.AddRevision
                            || AppEvent.EventName != EventType.AddVersion
                            || AppEvent.EventName != EventType.AddToFolder
                            || AppEvent.EventName != EventType.Classify
                            || AppEvent.EventName != EventType.Delete
                            || AppEvent.EventName != EventType.Expiration
                            || AppEvent.EventName != EventType.Protocol
                            || AppEvent.EventName != EventType.Publish
                            )
                        {

                            _ = Task.Run(async () =>
                                {
                                    try
                                    {
                                        await workflow.SendMessage(AppEvent.EventName, doc.Id.ToString(), variables); //.GetAwaiter().GetResult();
                                    }
                                    catch (Exception ex)
                                    {

                                        logger.LogError(ex, "Invio evento '" + AppEvent.EventName + "' su documento Id." + doc.Id);
                                    }
                                });
                        }
                    }
                    //}
                    if (AppEvent.EventName == EventType.Delete)
                    {
                        try
                        {
                            await workflow.CancelProcessInstance(doc.Id);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Cancellazione documento Id." + doc.Id);
                        }
                    }
                }

                if (AppEvent.Variables.ContainsKey("Task"))
                {
                    // Eventi su Task, Applicazione, ...
                    var TaskItem = (TaskItemInfo)AppEvent.Get<TaskItemInfo>("Task");
                    var jobKey = TaskItem.ExecutionId;//.ProcessId;
                    if (!String.IsNullOrEmpty(jobKey))
                    {
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                if (AppEvent.EventName == EventType.TaskExecuted || AppEvent.EventName == EventType.Approval || AppEvent.EventName == EventType.Refuse)
                                {
                                    logger.LogDebug("Completamento Job:" + jobKey);
                                    var v = AppEvent.Variables.ContainsKey("FormData") ? AppEvent.Variables["FormData"].ToString() : "";
                                    if (!string.IsNullOrEmpty(v) && v != "[]" && v != "{}")
                                        await workflow.SetVariables(TaskItem.ProcessId, v);
                                    await workflow.CompleteJob(jobKey, variables);
                                }
                                //// Verifico se ci sono processi in attesa di un evento "Task.*"
                                //// await workflow.SendMessage(AppEvent.EventName, TaskItem.Id.ToString(), variables);
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Invio evento '" + AppEvent.EventName + "' su Task Id." + TaskItem.Id);
                            }
                        });
                    }
                }

                if (AppEvent.Variables.ContainsKey("UserTaskId"))
                {
                    // Eventi su Task, Applicazione, ...
                    UserTaskInfo UserTask = (UserTaskInfo)AppEvent.Get<UserTaskInfo>("UserTask");
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await workflow.SendMessage(AppEvent.EventName, UserTask.TaskItemInfo.Id.ToString(), variables);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Invio evento '" + AppEvent.EventName + "' su UserTask Id." + UserTask.TaskItemInfo.Id);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Evento di Workflow  '" + AppEvent.EventName + "'");
            }
        }
    }
}
