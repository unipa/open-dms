using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;

namespace Web.Pages.Shared.Components.ProcessInfoView
{


    public class ProcessInfoViewComponent : ViewComponent
    {
        public ILoggedUserProfile LoggedUserProfile { get; }

        //private readonly IUserService userService;
        //private readonly IDocumentService documentService;
        //private readonly IUserTaskService userTaskService;
        //private readonly IProcessInstanceRepository processInstanceRepository;
        //private readonly IWorkflowEngine WorkflowEngine;

        public class ProcessInfoModel
        {
            public int DocumentId { get; set; }
            public bool IsAdmin { get; set; } = false;
        }

        public ProcessInfoViewComponent(
            ILoggedUserProfile loggedUserProfile
            //IUserService userService,
            //IDocumentService documentService,
            //IUserTaskService userTaskService,
            //IProcessInstanceRepository processInstanceRepository,
            //IWorkflowEngine WorkflowEngine
)
        {
            LoggedUserProfile = loggedUserProfile;
            //this.userService = userService;
            //this.documentService = documentService;
            //this.userTaskService = userTaskService;
            //this.processInstanceRepository = processInstanceRepository;
            //this.WorkflowEngine = WorkflowEngine;
        }


        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            //List<ProcessInstanceViewModel> plist = new();
            //var u = await userService.GetUserProfile(SpecialUser.SystemUser);
            //var u2 = await userService.GetUserProfile(this.User.Identity.Name);
            //bool IsWorkflowArchitect = u2.Roles.Any(r => r.Id == SpecialUser.WorkflowArchitect || r.Id == SpecialUser.AdminRole);

            //foreach(var pi in await processInstanceRepository.GetByDocumentId(Id))
            //{
            //    ProcessInstanceViewModel p = new();
            //    p.Id = pi.ProcessInstanceId;
            //    p.DocumentId = Id;
            //    p.EventName = pi.EventName;
            //    p.DefinitionKey = pi.ProcessKey;
            //    p.DefinitionId = pi.ProcessDefinitionId;
            //    var doc = await documentService.Get(pi.ProcessDefinitionId);
            //    p.Version = doc.Image.VersionNumber + "." + doc.Image.RevisionNumber;
            //    p.Description = doc.Description;
            //    p.Icon = doc.Icon;
            //    p.IconColor = doc.IconColor;
            //    p.StartTime = pi.StartDate.ToString("dd/MM/yyyy HH:mm");
            //    p.StartUserId = pi.StartUser;
            //    p.StartUserName = await userService.GetName(pi.StartUser);
            //    var pinfo = await WorkflowEngine.GetInstanceById(p.Id);
            //    p.Problems = IsWorkflowArchitect ? pinfo.incidents : new();
            //    p.Variables  = IsWorkflowArchitect ? pinfo.variables : new();
            //    foreach (var i in await userTaskService.GetAllDocumentTasks(Id, u))
            //    {
            //        var ut = await userTaskService.GetById(i.Id, u);
            //        if (ut != null)
            //        {
            //            //if (!string.IsNullOrEmpty(ut.TaskItemInfo.Process.Id))
            //            if (i.Status != ExecutionStatus.Executed && i.Status != ExecutionStatus.Validated)
            //                p.ActiveTasks.Add(await userTaskService.GetById(i.Id, u));
            //            else
            //                p.ClosedTasks.Add(await userTaskService.GetById(i.Id, u));
            //        }
            //    }
            //    plist.Add(p);
            //}

            //return View(plist);
            var model = new ProcessInfoModel()
            {
                DocumentId = Id,
                IsAdmin = LoggedUserProfile.Get().GlobalRoles.Any(r => r.Id.Equals(SpecialUser.AdminRole))
            };
            return View(model);
        }

    }
}
