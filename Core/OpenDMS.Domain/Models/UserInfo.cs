using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.Models;

public class UserProfile
{
    public string userId { get; set; } = "";
    public string LoggedUser { get; set; } = "";
    public List<LookupTable> Roles { get; set; } = new List<LookupTable>();
    public List<LookupTable> GlobalRoles { get; set; } = new List<LookupTable>();
    public List<LookupTable> Groups { get; set; } = new List<LookupTable>();
    public Dictionary<string, AuthorizationType> Permissions { get; set; } = new();

    public User UserInfo { get; set; } = new User() { };

    public List<Company> Companies{ get; set; } = new();

    private bool cached = false;
    private bool _isService = false;
    public bool IsService { 
        get {
            if (!cached)
            {
                _isService = Roles.FirstOrDefault(r => r.Id == SpecialUser.ServiceRole) != null;
                cached = true;
            }
            return _isService;
        }
    }  


    public static UserProfile SystemUser ()
    {
        return new UserProfile() { userId = SpecialUser.SystemUser, LoggedUser = SpecialUser.SystemUser, UserInfo = User.SystemUser() };
    }

}

