using OpenDMS.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Services
{
    public interface IAuthorizationService
    {
        AuthorizationType GetPermission(string Resource, string Scope);

    }
}
