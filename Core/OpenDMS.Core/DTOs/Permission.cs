using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    public class Permission
    {
        public string PermissionId { get; set; }
        public string Label { get; set; } = "";
        public AuthorizationType Authorization { get; set; } = AuthorizationType.None;

        public Permission()
        {
        }

        public Permission(string permissionType, string permissionName, AuthorizationType authorization)
        {
            this.PermissionId = permissionType;
            this.Label = permissionName; // PermissionType.Name[this.PermissionId]; 
            this.Authorization = authorization;
        }
    }
}
