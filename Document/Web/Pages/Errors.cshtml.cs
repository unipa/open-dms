using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorsModel : PageModel
    {
        public string? ErrorIcon { get; set; }
        public string RequestId { get; set; }
        public string ErrorTitle { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorsModel> _logger;

        public ErrorsModel(ILogger<ErrorsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ErrorIcon = "fa-bug";
            //RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            if (HttpContext.Response.StatusCode == 401 || HttpContext.Response.StatusCode == 403)
                ErrorIcon = "fa-ban";
            else if (HttpContext.Response.StatusCode == 404)
                ErrorIcon = "fa-search";

            if (HttpContext.Response.StatusCode == 401 || HttpContext.Response.StatusCode == 403)
                ErrorTitle = "Accesso Negato";
            else if (HttpContext.Response.StatusCode == 404)
                ErrorTitle = "Pagina non trovata";
            else
                ErrorTitle = "Errore Imprevisto";

            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (feature != null)
            {
                RequestId = feature.Error.Message;
                //_logger.LogException(feature.Error);
            }
        }
    }
}