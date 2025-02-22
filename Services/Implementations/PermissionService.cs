using Microsoft.EntityFrameworkCore;
using RBAC.Data;
using RBAC.Services.Interfaces;

namespace RBAC.Services.Implementations;

public class PermissionService(DataContext context) : IPermissionService
{
    public async Task<bool> UserHasPermissionAsync(int userId, string permissionName)
    {
        return await context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.UserRoles)
            .Select(ur => ur.Role)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission)
            .AnyAsync(p => p.Name == permissionName);
    }

    public async Task<bool> UserHasRoleAsync(int userId, string roleName)
    {
        return await context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == roleName);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
    {
        return await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(int userId)
    {
        return await context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.UserRoles)
            .Select(ur => ur.Role)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToListAsync();
    }
}
