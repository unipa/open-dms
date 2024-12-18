using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Domain.Enumerators;

namespace Web.Pages
{
    [Authorize]
    public class LoadContentModel : PageModel
    {
        private readonly ILogger<LoadContentModel> _logger;
        private readonly IConfiguration _config;

        public LoadContentModel(ILogger<LoadContentModel> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public string? host { get; set; } = "";
        public ArchivingStrategy ArchStrategy { get; set; }
        public int DocId { get; set; }
        public string DocTypeName { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(ArchivingStrategy ArchStrategy, int DocId, string DocTypeName)
        {
            try
            {
                this.DocTypeName = DocTypeName;
                this.ArchStrategy = ArchStrategy;
                this.DocId = DocId;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message.ToString();
            }

        }
    }
}
