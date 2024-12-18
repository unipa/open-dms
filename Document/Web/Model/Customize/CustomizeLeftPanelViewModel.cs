using OpenDMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.Model.Customize
{
    public class CustomizeLeftPanelViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<OptionPage> OptionPages { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string Icon { get; set; }

    }

    public class OptionPage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Group { get; set; }
        public string Href { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
    }

}
