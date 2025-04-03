using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

public class AdminController : Controller
{
    [Authorize(Roles = "Admin")]
    public IActionResult AdminDashboard()
    {
        return View();
    }

    // This action is accessible by all authenticated users
    public IActionResult Users()
    {
        return View();
    }
}
