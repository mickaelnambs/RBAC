using System;
using System.ComponentModel.DataAnnotations;

namespace RBAC.Entities;

public class Permission : BaseEntity
{
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
    [StringLength(255)]
    public string? Description { get; set; }
    [Required]
    [StringLength(100)]
    public required string Module { get; set; } 

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}
