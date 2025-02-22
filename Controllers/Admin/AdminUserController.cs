using Microsoft.AspNetCore.Mvc;
using RBAC.Authorization;

namespace RBAC.Controllers.Admin;

[Route("api/admin/[controller]")]
public class AdminUserController : AdminBaseController
{
    [HttpGet]
    [RequirePermission("users.view")]
    public IActionResult GetUsers()
    {
        return Ok(new { message = "User list retrieved" });
    }

    [HttpPost]
    [RequirePermission("users.create")]
    public IActionResult CreateUser()
    {
        return Ok(new { message = "User created" });
    }

    [HttpPut("{id}")]
    [RequirePermission("users.edit")]
    public IActionResult UpdateUser(int id)
    {
        return Ok(new { message = $"User {id} updated" });
    }

    [HttpDelete("{id}")]
    [RequirePermission("users.delete")]
    public IActionResult DeleteUser(int id)
    {
        return Ok(new { message = $"User {id} deleted" });
    }
}
