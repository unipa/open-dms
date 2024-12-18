namespace OpenDMS.Workflow.API.DTOs
{

    public class CustomChangeItems
    {
        public string name { get; set; }

        public List<CustomElement> items { get; set; } = new();
    }
}
