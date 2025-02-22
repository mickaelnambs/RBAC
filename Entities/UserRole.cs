using System;

namespace RBAC.Entities;

public class UserRole
{
    public int UserId { get; set; }
    public required User User { get; set; }
    
    public int RoleId { get; set; }
    public required Role Role { get; set; }
    
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
