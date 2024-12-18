using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;

namespace Web.Pages.Help
{
    public class FAQModel : PageModel
    {
        public const string Supporto_Tecnico = "Supporto Tecnico";

        private IWebHostEnvironment hostEnvironment;
        private readonly ILoggedUserProfile loggedUserProfile;

        public List<string> ReleaseNotes { get; set; }
        public bool IsFAQAdmin { get; set; } = false;

        public FAQModel(IWebHostEnvironment hostEnvironment,
            ILoggedUserProfile loggedUserProfile)
        {
            this.hostEnvironment = hostEnvironment;
            this.loggedUserProfile = loggedUserProfile;
        }

        public void OnGet()
        {
            GetReleaseNotes();
            var u = loggedUserProfile.Get();
            IsFAQAdmin = u.GlobalRoles.Any(r=>string.Compare( r.Id, "FAQAdmin", true) == 0);
        }

        private void GetReleaseNotes()
        {
            var files = Directory.GetFiles(Path.Combine(hostEnvironment.WebRootPath, "ReleaseNotes"), "*.html").OrderByDescending(f => f);
            ReleaseNotes = files.Select(s => Path.GetFileNameWithoutExtension(s)).Where(f => !f.Equals(Supporto_Tecnico)).ToList();
        }

    }
}
