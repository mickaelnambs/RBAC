using RBAC.Authorization;
using RBAC.Services.Implementations;
using RBAC.Services.Interfaces;

namespace RBAC.Extensions;

public static class RbacServiceExtension
{
    public static IServiceCollection AddRbacServices(this IServiceCollection services)
    {
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddPermissionPolicies();
        
        return services;
    }
}