using HomeOwner.Data;
using HomeOwner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeOwner.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly AppDbContext _context;

        public AnnouncementsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Announcements (Everyone can view)
        public async Task<IActionResult> Index()
        {
            var announcements = await _context.Announcements
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return View(announcements);
        }

        // GET: Announcements/Details/5 (Everyone can view)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound();

            return View(announcement);
        }

        // GET: Announcements/Create (Only Admins)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                                  ?? throw new InvalidOperationException("User ID cannot be null.");

                    announcement.CreatedAt = DateTime.Now;
                    announcement.AdminId = userId;

                    _context.Add(announcement);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating announcement: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ModelState is invalid");
            }

            return View(announcement);
        }




        // GET: Announcements/Edit/5 (Only Admins)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound();

            return View(announcement);
        }

        // POST: Announcements/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Announcement announcement)
        {
            if (id != announcement.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    announcement.CreatedAt = DateTime.Now; // Update timestamp
                    _context.Update(announcement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Announcements.Any(e => e.Id == id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(announcement);
        }

        // GET: Announcements/Delete/5 (Only Admins)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound();

            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Method to fetch announcements for the layout
        public async Task<IActionResult> AnnouncementsPartial()
        {
            var announcements = await _context.Announcements
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return PartialView("_AnnouncementsPartial", announcements);
        }


    }
}
