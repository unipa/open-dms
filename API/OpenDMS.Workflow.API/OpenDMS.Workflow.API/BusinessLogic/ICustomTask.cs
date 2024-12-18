using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace OpenDMS.Workflow.API.BusinessLogic
{
    public interface ICustomTask
    {
        string JobType { get; set; }
        string TaskLabel { get; set; }
        string Icon { get; set; }
        string GroupName { get; set; }
        string[] AlternativeTasks { get; set; }

        string Inputs { get; set; }
        string Outputs { get; set; }

        int MaxJobs { get; set; } 
        int PollingInterval { get; set; } 
        int TimeOut { get; set; } 

        Task HandleJob(IJobClient jobClient, IJob job);
        Task Initialize();

        
        Task<TaskItem> PaletteItem();
        

    }
}
