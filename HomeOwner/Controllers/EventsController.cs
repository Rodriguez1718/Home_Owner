using HomeOwner.Data;
using HomeOwner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeOwner.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Event ev)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ev);
        }

        // Display events in JSON format for FullCalendar
        [HttpGet]
        public JsonResult GetEvents()
        {
            var events = _context.Events.Select(e => new
            {
                id = e.Id,
                title = e.Title,
                start = e.StartDate,
                end = e.EndDate,
                backgroundColor = e.BackgroundColor ?? "#3788d8" // fallback color
            }).ToList();

            return new JsonResult(events);
        }

        // Update Event
        [HttpPost]
        public async Task<IActionResult> UpdateEventDate([FromBody] UpdateEventDTO updated)
        {
            var existingEvent = await _context.Events.FindAsync(updated.Id);
            if (existingEvent == null)
                return NotFound();

            // Update only the dates
            existingEvent.StartDate = updated.Start;
            existingEvent.EndDate = updated.End ?? updated.Start;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Updated successfully" });
        }


        // Edit Event
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound();

            return View(ev);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event ev)
        {
            if (id != ev.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ev);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Events.Any(e => e.Id == id))
                        return NotFound();

                    throw;
                }
            }
            return View(ev);
        }

        // Delete Event
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound();

            return View(ev);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Event Details
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound();

            return View(ev);
        }
    }
}
