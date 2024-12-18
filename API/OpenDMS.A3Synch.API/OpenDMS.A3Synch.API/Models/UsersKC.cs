using System.ComponentModel.DataAnnotations;

namespace A3Synch.Models
{
    public class UsersKC
    {
        [StringLength(64)]
        public string username { get; set; }
    }

}