using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;
using System.IO.Compression;
using Web.Pages.Folders;

namespace Web.Pages.Shared.Components.Viewer_Zip
{
    public class ZipInfo
    {
        public int DocumentId { get; set; }
        public string Title { get; set; }
        public string ErrorMessage { get; set; }
        public List<FolderInfo> Entries { get; set; } = new();
    }


    public class Viewer_Zip : ViewComponent
    {
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IViewManager viewManager;
        private readonly IViewServiceResolver viewMapper;


        public Viewer_Zip(IDocumentService documentService,
            ILoggedUserProfile userContext,
            IViewManager viewManager,
            IViewServiceResolver viewMapper)
        {
            this.documentService = documentService;
            this.userContext = userContext;
            this.viewManager = viewManager;
            this.viewMapper = viewMapper;
        }



        public async Task<IViewComponentResult> InvokeAsync(IndexModel sr)
        {
            var u = userContext.Get();
            var content = await documentService.GetContent(sr.Document.Image.Id);
            ZipInfo Z = new();
            Z.Title = sr.Document.Image.OriginalFileName;
            Z.DocumentId = sr.DocumentId;
            try
            {
                using (var m = new MemoryStream(content))
                {
                    using (var zip = new ZipArchive (m, ZipArchiveMode.Read))
                    {
                        foreach (var e in zip.Entries)
                        {
                            if (!String.IsNullOrEmpty(e.Name))
                            {
                                FolderInfo F = new FolderInfo();
                                F.Name = e.Name;
                                F.Path = Path.GetDirectoryName(e.FullName).Split(Path.PathSeparator).Select(p => new LookupTable { Id = p, Description = p }).ToList();
                                F.Nr = FormatFileSize(e.Length);
                                if (e.Length > 0)
                                    F.DocumentType = (e.CompressedLength / e.Length * 100).ToString("#.##") + "%";
                                else
                                    F.DocumentType = "";
                                F.Date = e.LastWriteTime.ToString("dd/MM/yyy HH:mm");
                                Z.Entries.Add(F);
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                Z.ErrorMessage = ex.Message;
            }
            return View(Z);
        }

        public string FormatFileSize(long fileSize)
        {
            if (fileSize < 1000)
            {
                return fileSize.ToString() + " B";
            }
            else
                if (fileSize < 1000_000)
            {
                return (fileSize / 1000).ToString("##0") + " K";
            }
            else
                if (fileSize < 1000_000_000)
            {
                return (fileSize / 1000_000).ToString("##0") + " M";
            }
            else
            {
                return (fileSize / 1000_000_000).ToString("##0") + " G";
            }
        }

    }
}
