using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalPhotoPrintingSystem.Data;

namespace DigitalPhotoPrintingSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var custId = HttpContext.Session.GetInt32("CustId");
            if (custId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _context.Customers.FindAsync(custId);
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _context.PurchaseOrders
                .Where(o => o.CustId == custId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            ViewBag.Customer = customer;
            ViewBag.TotalOrders = orders.Count;
            ViewBag.TotalSpent = orders.Sum(o => o.TotalAmount);
            ViewBag.RecentOrders = orders.Take(5);

            return View();
        }

        public async Task<IActionResult> OrderHistory()
        {
            var custId = HttpContext.Session.GetInt32("CustId");
            if (custId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _context.PurchaseOrders
                .Where(o => o.CustId == custId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
    }
}
