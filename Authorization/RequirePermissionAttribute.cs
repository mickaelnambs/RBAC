using Microsoft.AspNetCore.Authorization;

namespace RBAC.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute(string permission) : AuthorizeAttribute("Permission")
{
    public string Permission { get; } = permission;
}
