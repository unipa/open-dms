using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Models;
using Web.DTOs;

namespace Web.BL.Interface
{
    public interface ICustomizeBL
    {
        Task<User> GetUser();
        UserProfile GetUserProfile();
        Task<List<string>> GetRoles();
        Task<List<string>> GetGroups();
        Task<List<ACLPermission_DTO>> GetAuthorizations();
        Task<List<ACLPermission_DTO>> GetAuthorizationsForUser(string userId);
        Task<string> GetUserAttribute(string name);
        Task UpdateUser(User user);
        Task SetUserAttribute(string name, string value);
        Task SetAvatar(string imagePath);
        Task<List<Tuple<string, string>>> GetUploadedAvatars();
        Task UploadAvatar(IFormFile image);
        Task DeleteAvatar(string guid);
        Task<List<ContactDigitalAddress>> GetUserMails();
        Task SetHandWrittenSign(string sign);
        Task DeleteHandWrittenSign();
        Task SetVisto(string sign);
        Task DeleteVisto();
    }
}