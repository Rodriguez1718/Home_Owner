using HomeOwner.Data;
using HomeOwner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeOwner.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceRequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequest
        // GET: ServiceRequest
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID

            if (User.IsInRole("Admin"))
            {
                // Admins can view all service requests with user names
                var allRequests = await _context.ServiceRequests
                    .Select(sr => new
                    {
                        sr.Id,
                        sr.Title,
                        sr.Description,
                        sr.RequestDate,
                        sr.Status,
                        sr.ApprovalDate,
                        CreatedByName = _context.Users
                            .Where(u => u.Id == sr.CreatedBy)
                            .Select(u => u.FullName)
                            .FirstOrDefault() ?? "Unknown"
                    })
                    .ToListAsync();

                // Map the result to the ServiceRequest model
                var viewModel = allRequests.Select(sr => new ServiceRequest
                {
                    Id = sr.Id,
                    Title = sr.Title,
                    Description = sr.Description,
                    RequestDate = sr.RequestDate,
                    Status = sr.Status,
                    ApprovalDate = sr.ApprovalDate,
                    CreatedBy = sr.CreatedByName // Use the name instead of the ID
                });

                return View(viewModel);
            }
            else if (User.IsInRole("HomeOwner"))
            {
                // Homeowners can only view their own service requests
                var userRequests = await _context.ServiceRequests
                    .Where(sr => sr.CreatedBy == userId)
                    .ToListAsync();
                return View(userRequests);
            }

            // If the user has no valid role, return access denied
            return Forbid();
        }



        // GET: ServiceRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequest/Create
        [Authorize(Roles = "HomeOwner")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequest/Create
        [HttpPost]
        [Authorize(Roles = "HomeOwner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                // Set the CreatedBy field to the logged-in user's ID
                serviceRequest.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                serviceRequest.Status = "Pending"; // Default status
                serviceRequest.RequestDate = DateTime.Now; // Set the request date

                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequest);
        }


        // GET: ServiceRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }
            return View(serviceRequest);
        }

        // POST: ServiceRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,RequestDate,Status,AssignedTo")] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestExists(serviceRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequest);
        }

        // GET: ServiceRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            serviceRequest.Status = "Approved";
            serviceRequest.ApprovedBy = User.Identity.Name; // Current user
            serviceRequest.ApprovalDate = DateTime.Now;

            _context.Update(serviceRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Decline(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            serviceRequest.Status = "Declined";
            serviceRequest.ApprovedBy = User.Identity.Name; // Current user
            serviceRequest.ApprovalDate = DateTime.Now;

            _context.Update(serviceRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
