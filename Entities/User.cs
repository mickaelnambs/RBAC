using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RBAC.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User : BaseEntity
{
    public required string Name { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];

}
