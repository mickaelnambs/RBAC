using Microsoft.AspNetCore.Mvc;
using RBAC.Authorization;

namespace RBAC.Controllers.Admin;

[Route("api/admin/[controller]")]
public class AdminOrderController : AdminBaseController
{
    [HttpGet]
    [RequirePermission("orders.view")]
    public IActionResult GetOrders()
    {
        return Ok(new { message = "Order list retrieved" });
    }

    [HttpGet("{id}")]
    [RequirePermission("orders.view")]
    public IActionResult GetOrderById(int id)
    {
        return Ok(new { message = $"Order {id} details retrieved" });
    }

    [HttpPost]
    [RequirePermission("orders.create")]
    public IActionResult CreateOrder()
    {
        return Ok(new { message = "Order created" });
    }

    [HttpPut("{id}")]
    [RequirePermission("orders.edit")]
    public IActionResult UpdateOrder(int id)
    {
        return Ok(new { message = $"Order {id} updated" });
    }

    [HttpDelete("{id}")]
    [RequirePermission("orders.delete")]
    public IActionResult DeleteOrder(int id)
    {
        return Ok(new { message = $"Order {id} deleted" });
    }
    
    [HttpPut("{id}/status")]
    [RequirePermission("orders.edit")]
    public IActionResult UpdateOrderStatus(int id, [FromBody] string status)
    {
        return Ok(new { message = $"Order {id} status updated to {status}" });
    }
}