namespace OpenDMS.Core.DTOs;

public enum PermissionStatus
{
    None = 0,
    Granted = 1,
    Denied = 2
}

public class DocumentInstancePermissions
{
    public PermissionStatus CanRead { get; set; }
    public PermissionStatus CanView { get; set; }
    public PermissionStatus CanEdit { get; set; }
    public PermissionStatus CanDelete { get; set; }
    public PermissionStatus CanAddImage { get; set; }
    public PermissionStatus CanRemoveImage { get; set; }
    public PermissionStatus CanShare { get; set; }
    public PermissionStatus CanProtocol { get; set; }
    public PermissionStatus CanAuthorize { get; set; }
}

public class DocumentInstancePermissionsWithAvatar : DocumentInstancePermissions
{
    public string AvatarId { get; set; }
    public string Name { get; set; }

}
