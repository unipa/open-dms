using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RemoteSignInfocert.Models
{
    public class UploadParameter
    {
        public string? SignRoom { get; set; }

        public string? FileName { get; set; }
        public string? FileContent { get; set; }

    }
}
