using Microsoft.AspNetCore.Mvc;
using RBAC.Authorization;

namespace RBAC.Controllers.Admin;

[Route("api/admin/[controller]")]
public class AdminProductController : AdminBaseController
{
    [HttpGet]
    [RequirePermission("products.view")]
    public IActionResult GetProducts()
    {
        return Ok(new { message = "Product list retrieved" });
    }

    [HttpPost]
    [RequirePermission("products.create")]
    public IActionResult CreateProduct()
    {
        return Ok(new { message = "Product created" });
    }

    [HttpPut("{id}")]
    [RequirePermission("products.edit")]
    public IActionResult UpdateProduct(int id)
    {
        return Ok(new { message = $"Product {id} updated" });
    }

    [HttpDelete("{id}")]
    [RequirePermission("products.delete")]
    public IActionResult DeleteProduct(int id)
    {
        return Ok(new { message = $"Product {id} deleted" });
    }
}