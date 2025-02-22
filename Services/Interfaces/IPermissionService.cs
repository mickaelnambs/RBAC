namespace RBAC.Services.Interfaces;

public interface IPermissionService
{
    Task<bool> UserHasPermissionAsync(int userId, string permissionName);
    Task<bool> UserHasRoleAsync(int userId, string roleName);
    Task<IEnumerable<string>> GetUserRolesAsync(int userId);
    Task<IEnumerable<string>> GetUserPermissionsAsync(int userId);
}
