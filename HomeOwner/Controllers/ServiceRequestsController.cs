// Controllers/ServiceRequestsController.cs
using Microsoft.AspNetCore.Mvc;
using HomeOwner.Data;
using HomeOwner.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HomeOwner.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceRequests.ToListAsync());
        }

        // GET: ServiceRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequest);
        }

        // Additional actions: Details, Edit, Delete can be added similarly
    }
}
