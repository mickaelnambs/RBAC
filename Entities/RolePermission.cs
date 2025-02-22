namespace RBAC.Entities;

public class RolePermission
{
    public int RoleId { get; set; }
    public required Role Role { get; set; }
    
    public int PermissionId { get; set; }
    public required Permission Permission { get; set; }
    
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
