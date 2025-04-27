using Microsoft.AspNetCore.Mvc;
using HomeOwner.Models;
using HomeOwner.Data;
using Microsoft.EntityFrameworkCore; // Assuming you have a DbContext

public class FacilityReservationController : Controller
{
    private readonly AppDbContext _context;

    public FacilityReservationController(AppDbContext context)
    {
        _context = context;
    }

    // View available facilities
    public IActionResult Facilities()
    {
        var facilities = _context.Facilities.ToList();
        return View(facilities);
    }

    // View reservations list (Admin/Staff)
    public IActionResult Index()
    {
        var reservations = _context.Reservations
                            .Include(r => r.Facility)
                            .Include(r => r.User)
                            .ToList();
        return View(reservations);
    }

    // Make a reservation (form GET)
    public IActionResult Create()
    {
        ViewBag.Facilities = _context.Facilities.ToList();
        return View();
    }

    // Save a reservation (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Reservation reservation)
    {
        if (ModelState.IsValid)
        {
            reservation.Status = "Pending";
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(reservation);
    }

    // Approve Reservation
    public IActionResult Approve(int id)
    {
        var reservation = _context.Reservations.Find(id);
        if (reservation != null)
        {
            reservation.Status = "Approved";
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    // Decline Reservation
    public IActionResult Decline(int id)
    {
        var reservation = _context.Reservations.Find(id);
        if (reservation != null)
        {
            reservation.Status = "Declined";
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
