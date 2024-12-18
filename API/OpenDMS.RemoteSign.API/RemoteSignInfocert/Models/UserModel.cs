using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RemoteSignInfocert.Models
{
    [PrimaryKey("UserName")]
    public class UserModel
    {
        [StringLength(64)]
        public string? UserName { get; set; }
        [StringLength(255)]
        public string? NomeCompleto { get; set; }
        [StringLength(255)]
        public string? Alias { get; set; }

    }
}
