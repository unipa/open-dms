using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.API.DTOs;

namespace OpenDMS.MailSpooler.API.Pages
{

   // [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IUserService userService;

        public IndexModel(IConfiguration config, IUserService userService)
        {
            this.config = config;
            this.userService = userService;
        }

        public string? DocumentApi { get; set; } = "";
        public string? UserServiceApi { get; set; } = "";
        public string? MailSpoolerApi { get; set; } = "";
        public string token { get; set; }
        public string UserId { get; set; }
        public IEnumerable<SelectListItem> UserAddresses { get; set; }
        public bool NewMessage { get; set; } = true;
        public MailMessage_DTO Message { get; set; } = new();

        public async Task OnGetAsync(string? Id)
        {
            UserId = User.Identity.Name;
            DocumentApi = config["Endpoint:DocumentService"];
            UserServiceApi = config["Endpoint:UserService"];
            MailSpoolerApi = config["Endpoint:MailSpoolerService"];




            token = await HttpContext.GetTokenAsync("access_token");

            UserAddresses = (await userService.GetAllContactDigitalAddress(UserId)).Select(x =>
                    new SelectListItem
                    {
                        Text = x.Address,
                        Value = x.Address,
                    }).ToList();

            if (!string.IsNullOrEmpty(Id))
            {
                NewMessage = false;
                Message = GetMessage(Id);   
            }


        }

        public MailMessage_DTO GetMessage(string mailMessageId)
        {
            MailMessage_DTO MailMessage = new MailMessage_DTO();
            MailMessage.Id = mailMessageId;
            MailMessage.FromAddress = "admin@local";
            MailMessage.To = new() { "Destinatario1@test.com", "Destinatario2@test.com", "Destinatario3@test.com" };
            MailMessage.CC = new() { "CC1@test.com", "CC2@test.com", "CC3@test.com" };
            MailMessage.CCr = new() { "CCr1@test.com", "CCr2@test.com", "CCr3@test.com" };
            MailMessage.Subject = "Test Oggetto";
            MailMessage.Body = "<h1> Test Body </h1>";
            MailMessage.Attachments = new() { 1, 2, 3 };
            MailMessage.IncludePDFPreview = true;
            return MailMessage;
        }
    }
}
