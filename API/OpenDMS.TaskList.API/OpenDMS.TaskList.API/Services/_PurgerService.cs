using OpenDMS.Core.Interfaces;

namespace OpenDMS.Tasklist.API.Services
{
    public class _PurgerService
    {
        private readonly IUserTaskService taskService;
        private Timer timer;

        public _PurgerService(IUserTaskService taskService)
        {
            this.taskService = taskService;
            DateTime lastday = DateTime.MinValue.Date;
            timer = new Timer(async (data) =>
            {
                if (DateTime.UtcNow.Date > lastday)
                {
                    lastday = DateTime.UtcNow.Date;
                    await taskService.RemoveOrphanTasks(lastday.AddDays(-1));
                }
            }, null, 60_000, 3600_000);
        }

    }
        
}
        
