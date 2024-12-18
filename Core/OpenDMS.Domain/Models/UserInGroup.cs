using OpenDMS.Domain.Entities.OrganizationUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Models
{
    public class UserInGroup
    {
        public int Id { get; set; }
        public string UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public OrganizationNode NodeInfo { get; set; } = null;

        public UserInGroup()
        {
            
        }

        public UserInGroup(UserGroupRole user)
        {
            this.StartDate = user.StartISODate > 0 ? new DateTime(user.StartISODate / 10000, user.StartISODate / 100 % 100, user.StartISODate % 100) : null;
            this.EndDate = user.EndISODate > 0 && user.EndISODate < 99999999 ? new DateTime(user.EndISODate / 10000, user.EndISODate / 100 % 100, user.EndISODate % 100) : null;
            this.UserGroupId = user.UserGroupId;
            this.UserGroup = new UserGroup() { Name = user.UserGroup?.Name, ShortName = user.UserGroup?.ShortName };
            this.UserId = user.UserId;
            this.RoleId = user.RoleId;
            this.Id = user.Id;
        }
    }
}
