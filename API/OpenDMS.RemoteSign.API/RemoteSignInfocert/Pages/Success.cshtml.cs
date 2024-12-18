using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RemoteSignInfocert.Pages
{
    [IgnoreAntiforgeryToken]
    public class SuccessModel : PageModel
    {
        public string Result { get; set; }
        public void OnGet(string Result)
        {
            this.Result = Result;
        }
    }
}
