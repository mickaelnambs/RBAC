using System.ComponentModel.DataAnnotations;

namespace RBAC.DTOs;

public class RegisterDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }

    [StringLength(32, MinimumLength = 6)]
    public required string Password { get; set; }
}
