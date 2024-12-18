namespace OpenDMS.Workflow.API.DTOs
{


    public class CustomElement
    {
        public string label { get; set; }
        public string actionName { get; set; }

        public string className { get; set; }
        public CustomTaskTargetType target { get; set; } = new CustomTaskTargetType();

        public List<string> Inputs { get; set; }
        public List<string> Outputs { get; set; }

    }
}
