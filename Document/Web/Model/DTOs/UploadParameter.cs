using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Web.Model.DTOs
{
    public class UploadParameter
    {
        public string? SignRoom { get; set; }
        public string? FileName { get; set; }
        public string? FileContent { get; set; }

    }
}
