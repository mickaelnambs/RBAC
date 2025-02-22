using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RBAC.Data;
using RBAC.Entities;
using RBAC.Services.Interfaces;

namespace RBAC.Services.Implementations;

public class AuthService(DataContext context, IConfiguration configuration, IPermissionService permissionService) : IAuthService
{
    public async Task<(bool Success, string Token, User user)> AuthenticateAsync(string email, string password)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return (false, null, null);
        }

        if (!VerifyPassword(password, user.Password))
        {
            return (false, null, null);
        }

        var roles = await permissionService.GetUserRolesAsync(user.Id);
        var permissions = await permissionService.GetUserPermissionsAsync(user.Id);

        var token = GenerateJwtToken(user, roles, permissions);

        return (true, token, user);
    }

    public string GenerateJwtToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not found")));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
