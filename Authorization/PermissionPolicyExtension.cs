using System;
using Microsoft.AspNetCore.Authorization;

namespace RBAC.Authorization;

public static class PermissionPolicyExtension
{
    public static void AddPermissionPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Permission", policy =>
                policy.Requirements.Add(new PermissionRequirement()));
        });

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }
}
