using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using Web.Model;

namespace Web.Pages
{
    [Authorize]
    public class ReassignModel : PageModel
    {
        private readonly ILogger<ReassignModel> _logger;
        private readonly ILoggedUserProfile userContext;
        private readonly IUserTaskService taskService;
        private readonly IConfiguration _config;



        public ReassignModel(ILogger<ReassignModel> logger, 
            ILoggedUserProfile userContext,
            IUserTaskService taskService,
            IConfiguration config)
        {
            _logger = logger;
            this.userContext = userContext;
            this.taskService = taskService;
            _config = config;
        }

        public SharePanelViewModel ShareInfo { get; set; }
        public UserTaskInfo CurrentTask { get; set; }

        public async Task OnGetAsync(int tid)
        {
            ShareInfo = new SharePanelViewModel();
            var u = userContext.Get();
            try
            {
                ShareInfo.DocId = tid.ToString();


                CurrentTask = await taskService.GetById(tid, u);
                ShareInfo.Message = "";
                ShareInfo.To = CurrentTask.TaskItemInfo.Sender;
                ShareInfo.Cc = "";
                ShareInfo.Action = "*";
                var valori = Enum.GetValues(typeof(ActionRequestType));
                var Stringhe = Enum.GetNames(typeof(ActionRequestType));

            }
            catch (Exception ex) { ShareInfo.ErrorMessage = ex.Message.ToString(); }

        }
    }
}
