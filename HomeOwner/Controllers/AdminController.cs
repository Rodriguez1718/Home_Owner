using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HomeOwner.Data;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminDashboard()
    {
        // Total Homeowners
        var homeownerRoleId = await _context.Roles
            .Where(r => r.Name == "HomeOwner")
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        ViewBag.TotalHomeowners = await _context.UserRoles
            .Where(ur => ur.RoleId == homeownerRoleId)
            .CountAsync();

        // Total Requests
        //ViewBag.TotalRequests = await _context.ServiceRequests.CountAsync();

        // Total Reservations
        ViewBag.TotalReservations = await _context.Reservations.CountAsync();

        // Requests Over Time (Example: Group by Date)
        //var requestsOverTime = await _context.ServiceRequests
            //.GroupBy(r => r.CreatedAt.Date)
            //.Select(g => new { Date = g.Key, Count = g.Count() })
           // .OrderBy(g => g.Date)
           // .ToListAsync();

       // ViewBag.RequestsOverTimeLabels = requestsOverTime.Select(r => r.Date.ToString("yyyy-MM-dd")).ToList();
        //ViewBag.RequestsOverTimeData = requestsOverTime.Select(r => r.Count).ToList();

        return View();
    }

    // This action is accessible by all authenticated users
    public IActionResult Users()
    {
        return View();
    }
}
