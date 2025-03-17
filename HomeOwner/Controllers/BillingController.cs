using HomeOwner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HomeOwner.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeOwner.Controllers
{
    public class BillingController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly AppDbContext _context;

        public BillingController(UserManager<Users> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Billing()
        {
            var user = await _userManager.GetUserAsync(User);
            var billings = await _context.Billings.Where(b => b.UserId == user.Id).ToListAsync();
            return View(billings);
        }

        public async Task<IActionResult> Pay(int id)
        {
            var billing = await _context.Billings.FindAsync(id);
            if (billing == null || billing.IsPaid)
            {
                return NotFound();
            }
            return View(billing);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(Billing billing)
        {
            var user = await _userManager.GetUserAsync(User);
            var payment = new Payment
            {
                BillingId = billing.BillingId,
                UserId = user.Id,
                Amount = billing.Amount,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            billing.IsPaid = true;
            _context.Billings.Update(billing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Billing));
        }
    }
}
