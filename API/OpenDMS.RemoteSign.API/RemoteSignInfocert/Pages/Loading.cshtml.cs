using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemoteSignInfocert.Controllers;
using RemoteSignInfocert.Interfaces;
using RemoteSignInfocert.Models;
using RemoteSignInfocert.Models.VM;
using System.Linq.Expressions;

namespace RemoteSignInfocert.Pages
{
    [IgnoreAntiforgeryToken]
    public class LoadingModel : PageModel
    {
        private string _signRoomBasePath = "";
        private const string SIGN_ROOMS = "SignRooms";
        private readonly IWebHostEnvironment _hostEnv;
        private readonly ISignRoomDAO _signRoomDao;
        private readonly SignController _signController;

        public string signRoom { get; set; } = "";
        public string Status { get; private set; } = "";
        public string Username { get; private set; } = "";
        public string ErrorMessage { get; private set; }
        public List<FileListVM> ElencoFile { get; set; } = new List<FileListVM>();
        public LoadingModel(IWebHostEnvironment hostEnv, ISignRoomDAO signRoomDao, SignController signController)
        {
            _hostEnv = hostEnv;
            _signRoomBasePath = Path.Combine(_hostEnv.ContentRootPath, SIGN_ROOMS);
            _signRoomDao = signRoomDao;
            _signController = signController;
        }

        public IActionResult OnGet(string SignRoom)
        {
            if (string.IsNullOrEmpty(SignRoom))
                ErrorMessage = "Il campo signRoom non può essere nullo.";
            else
                this.signRoom = SignRoom;

            var sr = _signRoomDao.GetSignRoom(this.signRoom);

            this.Username = sr.UserName;

            if (sr.Status == SignRoomStatus.ReadyToSign || sr.Status == SignRoomStatus.FileUploaded)
                return RedirectToPage("Index", new { SignRoom, sr.UserName, Wait = true });

            if ((int)sr.Status > 4 && sr.Delivered == true)
                return RedirectToPage("Success", new { Result = sr.DeliveryResult });

            this.Status = _signController.CheckStatus(SignRoom);

            var signRoomPath = Path.Combine(_signRoomBasePath, SignRoom);

            if (Directory.Exists(signRoomPath))
            {
                var files = _signRoomDao.GetSignRoomFiles(signRoomPath, sr.UserName).ToArray();//Directory.GetFiles(signRoomPath);
                foreach (var fileInfo in files)
                {
                    var isErasable = false;
                    //ElencoFile.Add(new FileListVM(
                    //    /*non viene passato nell uploadFile*/"",
                    //    fileInfo.Descrizione/*Descrizione*/,
                    //    Path.GetExtension(Utility.RemoveDocIdPattern(fileInfo.TempFileName)).Replace(".", "")/*tipo(estensione)*/,
                    //    (decimal.Round((Decimal)fileInfo.Size / 1024 / 1024, 2)).ToString(),
                    //    ""/*non viene passato nell uploadFile*/,
                    //    isErasable
                    //));
                }
            }
            else
                ErrorMessage = "SignRoom non trovata!";

            return Page();
        }
    }
}
