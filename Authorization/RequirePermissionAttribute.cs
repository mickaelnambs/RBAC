using Microsoft.AspNetCore.Authorization;

namespace RBAC.Authorization;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute, IAuthorizationRequirementData
{
    public string Permission { get; }
    
    public RequirePermissionAttribute(string permission) : base("Permission")
    {
        Permission = permission;
    }
    
    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return new PermissionRequirement(Permission);
    }
}
