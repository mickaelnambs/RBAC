using Microsoft.AspNetCore.Mvc;
using RBAC.Authorization;

namespace RBAC.Controllers.Admin;

[Route("api/admin/[controller]")]
public class AdminReviewController : AdminBaseController
{
    [HttpGet]
    [RequirePermission("reviews.view")]
    public IActionResult GetReviews()
    {
        return Ok(new { message = "Review list retrieved" });
    }

    [HttpPost]
    [RequirePermission("reviews.create")]
    public IActionResult CreateReview()
    {
        return Ok(new { message = "Review created" });
    }

    [HttpPut("{id}")]
    [RequirePermission("reviews.edit")]
    public IActionResult UpdateReview(int id)
    {
        return Ok(new { message = $"Review {id} updated" });
    }

    [HttpDelete("{id}")]
    [RequirePermission("reviews.delete")]
    public IActionResult DeleteReview(int id)
    {
        return Ok(new { message = $"Review {id} deleted" });
    }
    
    [HttpGet("{id}")]
    [RequirePermission("reviews.view")]
    public IActionResult GetReviewById(int id)
    {
        return Ok(new { message = $"Review {id} details retrieved" });
    }
    
    [HttpPut("{id}/approve")]
    [RequirePermission("reviews.edit")]
    public IActionResult ApproveReview(int id)
    {
        return Ok(new { message = $"Review {id} has been approved" });
    }
    
    [HttpPut("{id}/reject")]
    [RequirePermission("reviews.edit")]
    public IActionResult RejectReview(int id)
    {
        return Ok(new { message = $"Review {id} has been rejected" });
    }
}