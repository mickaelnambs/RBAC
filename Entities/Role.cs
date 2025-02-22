using System.ComponentModel.DataAnnotations;

namespace RBAC.Entities;

public class Role : BaseEntity
{
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }
    [StringLength(255)]
    public string? Description { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}
