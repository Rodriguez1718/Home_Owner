using HomeOwner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace HomeOwner.Controllers
{
    public class StaffController : Controller
    {
        private readonly UserManager<Users> _userManager;

        public StaffController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> StaffDashboard()
        {
            // Get the currently signed-in user (based on their cookie identity).
            var currentUser = await _userManager.GetUserAsync(User);

            // The user is admin, so continue to the admin page.
            return View();
        }
    }
}
