using HomeOwner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HomeOwner.Data;

namespace HomeOwner.Controllers
{
    public class StaffController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly AppDbContext _context; // Add this line to define _context  

        public StaffController(UserManager<Users> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context; // Initialize _context in the constructor  
        }

        public async Task<IActionResult> StaffDashboard()
        {
            // Get the currently signed-in user (based on their cookie identity).  
            var currentUser = await _userManager.GetUserAsync(User);

            // The user is admin, so continue to the admin page.  
            return View();
        }

        public async Task<IActionResult> Announcements()
        {
            var announcements = await _context.Announcements
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return View(announcements);
        }
    }
}
