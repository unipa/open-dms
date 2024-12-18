namespace Web.Model.Admin
{
    public class AdminLeftMenuViewModel
    {
        public string UserId { get; set; }
        public List<Area> Aree { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string Icon { get; set; }

    }

    public class Area
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Group { get; set; }
        public string Href { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
    }

}
