using HomeOwner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace HomeOwner.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<Users> _userManager;

        public AdminController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> AdminDashboard()
        {
            // Get the currently signed-in user (based on their cookie identity).
            var currentUser = await _userManager.GetUserAsync(User);

            // If the user is not logged in or their Role isn't "Admin", deny access.
            if (currentUser == null || !currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // The user is admin, so continue to the admin page.
            return View();
        }
    }
}
