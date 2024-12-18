namespace Web.Model.Customize
{
    public class AvatarViewModel
    {
        public List<Tuple<string, string>> UploadedAvatars { get; set; } = new();
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string Icon { get; set; }
    }
}
