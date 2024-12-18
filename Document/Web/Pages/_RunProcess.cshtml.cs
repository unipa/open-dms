using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Utilities;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace Web.Pages
{
    public class RunProcessModel : PageModel
    {
        private readonly ISearchService searchService;
        private readonly IDocumentService documentService;
        private readonly IUserService userService;
        private readonly IVirtualFileSystemProvider virtualFileSystem;
        private readonly IFormService formService;
        private readonly IUserTaskService taskService;
        private readonly IAppSettingsRepository appSettings;
        private readonly IWorkflowEngine workflowEngine;
        private readonly ILoggedUserProfile userContext;

        public FormSchema formSchema { get; set; } = null;
        public DocumentInfo ProcessInfo { get; set; } = null;
        public long pik { get; set; } = 0;

        public Exception Exception { get; set; } = null;

        public User UserInfo { get; set; } = null;

        public RunProcessModel(
            ISearchService searchService,
            IDocumentService documentService,
            IUserService userService,
            IVirtualFileSystemProvider virtualFileSystem,
            IFormService formService,
            IUserTaskService taskService,
            IAppSettingsRepository appSettings,
            IWorkflowEngine workflowEngine,
            ILoggedUserProfile userContext
            )
        {
            this.searchService = searchService;
            this.documentService = documentService;
            this.userService = userService;
            this.virtualFileSystem = virtualFileSystem;
            this.formService = formService;
            this.taskService = taskService;
            this.appSettings = appSettings;
            this.workflowEngine = workflowEngine;
            this.userContext = userContext;
        }

        public async Task OnGet(int pid, int? pik)
        {

            var u = userContext.Get();
            var su = UserProfile.SystemUser();

            var p = await documentService.GetPermission(pid, u, PermissionType.CanExecute);
            if (p == null || p.Authorization != AuthorizationType.Granted) return;

            var doc = await documentService.Load(pid, su);
            var pimage = (await documentService.Images(doc.Id, su)).LastOrDefault(i=>i.SendingStatus == JobStatus.Completed);
            if (doc == null || doc.Id <= 0 || pimage == null) return;

            ProcessInfo = doc;
            if (string.IsNullOrEmpty(ProcessInfo.Icon)) ProcessInfo.Icon = "fa fa-cog";
            if (string.IsNullOrEmpty(ProcessInfo.IconColor)) ProcessInfo.IconColor = "#bbb";

            formSchema = await formService.GetByUid (doc.DocumentNumber, u);
            if (formSchema != null)
            {
                formSchema.Schema = formSchema.Schema
                    .Parse(ProcessInfo, "ProcessInfo");
            }
            else
            {
                await StartProcess(null, pid, u);
            }
        }

        /// <summary>
        /// Richiamato dal salvataggio di un Form HTML
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="Variables"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost (int pid, [FromBody] Dictionary<string,string> Variables)
        {
            var u = userContext.Get();

            var p = await documentService.GetPermission(pid, u, PermissionType.CanExecute);
            if (p == null || p.Authorization != AuthorizationType.Granted) return BadRequest("non sei autorizzato ad eseguire questo processo");

            var doc = await documentService.Load(pid, u);
            ProcessInfo = doc;

            await StartProcess(Variables, pid, u);
            return new JsonResult( pik);
        }

        private async Task StartProcess(Dictionary<string, string> Variables, int pid, UserProfile u)
        {
            var _Variables = new Dictionary<string, object>();
            if (Variables != null)
            {
                foreach (var v in Variables)
                {
                    _Variables[v.Key] = v.Value;
                }
            }

            var doc = await documentService.Load(pid, u);
            var pimage = (await documentService.Images(doc.Id, u)).LastOrDefault(i => i.SendingStatus == JobStatus.Completed);
            if (doc == null || doc.Id <= 0 || pimage == null) return;

            var processKey = doc.ExternalId;
            if (int.TryParse(processKey, out int processId))
                processKey = "ID" + processKey;

            _Variables["UserProfile"] = (u);
            _Variables["User"] = (UserInfo);
            _Variables["ProcessInfo"] = (ProcessInfo);
            //if (FormInfo != null)
            //    _Variables["FormInfo"] = (FormInfo);
            try
            {
                string JsonVariables = System.Text.Json.JsonSerializer.Serialize(_Variables);

                pik =  await workflowEngine.StartProcess(processKey, doc, u, EventType.Creation, JsonVariables);
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }
    }
}
 