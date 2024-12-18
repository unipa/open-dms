using OpenDMS.Domain.Models;
using System.Security.Claims;

namespace OpenDMS.Core.Interfaces
{
    public interface IUpdateIdentityService
    {
        Task<UserProfile> Update(ClaimsPrincipal Claim);
    }
}