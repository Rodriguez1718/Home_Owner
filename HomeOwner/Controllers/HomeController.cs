using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeOwner.Models;
using Microsoft.EntityFrameworkCore;
using HomeOwner.Data;

namespace HomeOwner.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context; // Added this line

    public HomeController(ILogger<HomeController> logger, AppDbContext context) // Updated constructor
    {
        _logger = logger;
        _context = context; // Assigning the injected context
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Listings()
    {
        return View();
    }

    public IActionResult Blog()
    {
        return View();
    }

    public async Task<IActionResult> Announcements()
    {
        var announcements = await _context.Announcements
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
        return View(announcements);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
