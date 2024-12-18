using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Help
{
    public class IndexModel : PageModel
    {
        public const string Supporto_Tecnico = "Supporto Tecnico";

        public List<string> ReleaseNotes { get; set; }
        public string ReleaseNoteBody { get; set; } = "";
        public string ReleaseNoteFileName { get; set; } = "";

        public IWebHostEnvironment hostEnvironment { get; }

        public IndexModel(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        private void GetReleaseNotes()
        {
            var files = Directory.GetFiles(Path.Combine(hostEnvironment.WebRootPath, "ReleaseNotes"), "*.html").OrderByDescending(f => f);
            ReleaseNotes = files.Select(s => Path.GetFileNameWithoutExtension(s)).Where(f => !f.Equals(Supporto_Tecnico)).ToList();
        }

        public void OnGet(string? releaseNote)
        {
            GetReleaseNotes();

            if (string.IsNullOrEmpty(releaseNote))
                releaseNote = Supporto_Tecnico;
            ReleaseNoteFileName = releaseNote;

            var fileName = Path.Combine(hostEnvironment.WebRootPath, "ReleaseNotes", releaseNote + ".html");
            ReleaseNoteBody = System.IO.File.ReadAllText(fileName);
        }

    }
}
