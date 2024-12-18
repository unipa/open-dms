using OpenDMS.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    public class ProfilePermissions
    {
        public string ProfileName { get; set; }
        public string ProfileId { get; set; }
        public ProfileType ProfileType { get; set; }

        public string Profile => ((int)this.ProfileType).ToString() + ProfileId;
        public List<Permission> Permissions { get; set; } = new();

    }
}
 