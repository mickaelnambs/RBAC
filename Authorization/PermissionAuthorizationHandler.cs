using Microsoft.AspNetCore.Authorization;
using RBAC.Services.Interfaces;
using System.Security.Claims;

namespace RBAC.Authorization;

public class PermissionAuthorizationHandler(IPermissionService permissionService) : AuthorizationHandler<PermissionRequirement>
{

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim?.Value is not string userIdString || !int.TryParse(userIdString, out int userId))
        {
            return;
        }

        var requiredPermission = context.Resource as string;
        if (string.IsNullOrEmpty(requiredPermission))
        {
            return;
        }

        if (await permissionService.UserHasPermissionAsync(userId, requiredPermission))
        {
            context.Succeed(requirement);
        }
    }
}
