using HomeOwner.Data;
using HomeOwner.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeOwner.Controllers
{
    public class FacilityController : Controller
    {
        private readonly AppDbContext _context;

        public FacilityController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Create Facility
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Facility
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Facility facility)
        {
            if (ModelState.IsValid)
            {
                // Add the new facility to the database
                _context.Facilities.Add(facility);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index)); // Redirect to the list of facilities
            }

            return View(facility); // Return the view with validation errors
        }

        // GET: Index (list all facilities)
        public IActionResult Index()
        {
            var facilities = _context.Facilities.ToList();
            return View(facilities); // Display all facilities
        }

        // EDIT GET
        public IActionResult Edit(int id)
        {
            var facility = _context.Facilities.Find(id);
            if (facility == null)
            {
                return NotFound();
            }
            return View(facility);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Facility facility)
        {
            if (id != facility.FacilityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Facilities.Update(facility);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(facility);
        }

        // DELETE GET
        public IActionResult Delete(int id)
        {
            var facility = _context.Facilities.Find(id);
            if (facility == null)
            {
                return NotFound();
            }
            return View(facility);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var facility = _context.Facilities.Find(id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
