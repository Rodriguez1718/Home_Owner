using HomeOwner.Data;
using HomeOwner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeOwner.Controllers
{
    [Authorize(Roles = "HomeOwner")]
    public class HomeReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public HomeReservationController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Facility list for reservation
        public async Task<IActionResult> Index()
        {
            var facilities = await _context.Facilities.ToListAsync();
            return View(facilities); // Views/HomeReservation/Index.cshtml
        }

        // GET: Show reservation form
        public async Task<IActionResult> Create(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }

            ViewBag.Facilities = await _context.Facilities.ToListAsync(); // Add this line

            var reservation = new Reservation
            {
                FacilityId = facility.FacilityId,
                Facility = facility
            };

            return View(reservation);
        }



        // POST: Submit reservation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) // Ensure user is not null
            {
                return Unauthorized(); // Return an appropriate response if user is null
            }

            // Log the data being submitted
            Console.WriteLine($"FacilityId: {reservation.FacilityId}");
            Console.WriteLine($"ReservationDate: {reservation.ReservationDate}");
            Console.WriteLine($"StartTime: {reservation.StartTime}");
            Console.WriteLine($"EndTime: {reservation.EndTime}");
            Console.WriteLine($"Purpose: {reservation.Purpose}");
            Console.WriteLine($"UserId: {reservation.UserId}");

            if (ModelState.IsValid)
            {
                // Ensure FacilityId is valid
                var facility = await _context.Facilities.FindAsync(reservation.FacilityId);
                if (facility == null)
                {
                    ModelState.AddModelError("FacilityId", "Invalid facility selected.");
                    ViewBag.Facilities = await _context.Facilities.ToListAsync();
                    return View(reservation);
                }

                reservation.UserId = user.Id; // Safe to access user.Id now
                reservation.CreatedAt = DateTime.Now;
                reservation.Status = "Pending";

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Reservation submitted successfully!";
                return RedirectToAction("Confirmation", new { id = reservation.ReservationId });
            }

            // Log validation errors to Visual Studio Output
            Console.WriteLine("ModelState is invalid. Logging errors:");
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Key: {entry.Key}, Error: {error.ErrorMessage}");
                }
            }

            ViewBag.Facilities = await _context.Facilities.ToListAsync();
            return View(reservation);
        }


        // GET: Confirmation page
        public async Task<IActionResult> Confirmation(int id)
        {
            Console.WriteLine($"Confirmation request received for Reservation ID: {id}");

            var reservation = await _context.Reservations
                .Include(r => r.Facility)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null)
            {
                Console.WriteLine($"Reservation with ID {id} not found.");
                return NotFound();
            }

            Console.WriteLine($"Reservation {id} found. Returning confirmation view.");
            return View(reservation);
        }



        // GET: Homeowner's reservation history
        public async Task<IActionResult> MyReservations()
        {
            var user = await _userManager.GetUserAsync(User);
            var myReservations = await _context.Reservations
                .Include(r => r.Facility)
                .Where(r => r.UserId == user.Id)
                .ToListAsync();

            return View(myReservations); // Views/HomeReservation/MyReservations.cshtml
        }
    }
}
