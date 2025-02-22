using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RBAC.Data;
using RBAC.DTOs;
using RBAC.Entities;
using RBAC.Services.Interfaces;

namespace RBAC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(DataContext context, 
    IAuthService authService, IPermissionService permissionService) : ControllerBase
{

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Email))
        {
            return BadRequest("Email is already taken");
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            Password = hashedPassword
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var customerRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");
        if (customerRole != null)
        {
            context.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = customerRole.Id,
                User = user,
                Role = customerRole
            });
            await context.SaveChangesAsync();
        }

        var roles = await permissionService.GetUserRolesAsync(user.Id);
        var permissions = await permissionService.GetUserPermissionsAsync(user.Id);

        var token = authService.GenerateJwtToken(user, roles, permissions);

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = token
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users
            .SingleOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null || !authService.VerifyPassword(loginDto.Password, user.Password))
        {
            return Unauthorized("Invalid email or password");
        }

        var roles = await permissionService.GetUserRolesAsync(user.Id);
        var permissions = await permissionService.GetUserPermissionsAsync(user.Id);

        var token = authService.GenerateJwtToken(user, roles, permissions);

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = token
        };
    }

    private async Task<bool> UserExists(string email)
    {
        return await context.Users.AnyAsync(x => x.Email == email);
    }
}
