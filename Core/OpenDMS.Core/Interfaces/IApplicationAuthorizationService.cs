using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces
{
    public interface _IApplicationAuthorizationService
    {
        Task<bool> HasPermission(string ApplicationFunction, UserProfile userInfo);
    }
}