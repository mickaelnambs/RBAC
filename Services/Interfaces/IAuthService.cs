using RBAC.Entities;

namespace RBAC.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string Token, User user)> AuthenticateAsync(string email, string password);
    string GenerateJwtToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions);
    bool VerifyPassword(string passwrd, string hashedPassword);
}
