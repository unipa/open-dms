namespace OpenDMS.Workflow.API.DTOs
{

    public class CustomElementItems
    {
        public string id { get; set; }
        public string name { get; set; }

        public List<CustomElement> items { get; set; } = new();
    }
}
