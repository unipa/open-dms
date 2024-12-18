
using OpenDMS.Workflow.API.DTOs.Palette;

namespace OpenDMS.Workflow.API.DTOs
{
    public class WorkflowEditorAppContext
    {
        public string UserId { get; set; }
        public string Profile { get; set; }

        public string UserName { get; set; }

        public bool CanDeploy { get; set; } = false;
        public bool CanCustomize { get; set; } = false;

        //        public string AdminServiceEndPoint { get; set; }
        //        public string SearchServiceEndPoint { get; set; }
        //        public string DocumentPreviewServiceEndPoint { get; set; }
        //        public string DocumentServiceEndPoint { get; set; }
        //        public string UISettingsServiceEndPoint { get; set; }
        //        public string UserServiceEndPoint { get; set; }

        //        public List<CustomChangeItems> CustomChangeItems { get; set; } = new ();
        //        public List<CustomElementItems> CustomElementItems { get; set; } = new();

        public List<TaskGroup> TaskGroups { get; set; } = new();

    }
}
