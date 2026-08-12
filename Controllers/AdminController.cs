using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalPhotoPrintingSystem.Data;
using DigitalPhotoPrintingSystem.Models;

namespace DigitalPhotoPrintingSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Helper Method for Session Validation
        private bool IsUserLoggedIn()
        {
            var custId = HttpContext.Session.GetString("CustId");
            var userName = HttpContext.Session.GetString("UserName");
            return !string.IsNullOrEmpty(custId) || !string.IsNullOrEmpty(userName);
        }

        // ==========================================
        // 0. ADMIN DASHBOARD
        // ==========================================
        public async Task<IActionResult> Dashboard()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                ViewBag.TotalOrders = await _context.PhotoOrders.CountAsync();
                ViewBag.TotalRevenue = await _context.PhotoOrders.SumAsync(o => (decimal?)o.TotalPrice) ?? 0m;
                ViewBag.TotalCustomers = await _context.Customers.CountAsync();

                var recentOrders = await _context.PhotoOrders
                    .OrderByDescending(o => o.OrderDate)
                    .ThenByDescending(o => o.Id)
                    .Take(10)
                    .AsNoTracking()
                    .ToListAsync();

                return View(recentOrders);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Dashboard error: " + ex.Message;
                return View(new System.Collections.Generic.List<PhotoOrder>());
            }
        }

        // ==========================================
        // 1. MANAGE ORDERS (FIXED DIRECT VIEW PATH)
        // ==========================================
        public async Task<IActionResult> ManageOrders()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var orders = await _context.PhotoOrders
                    .OrderByDescending(o => o.OrderDate)
                    .ThenByDescending(o => o.Id)
                    .AsNoTracking()
                    .ToListAsync();

                // Direct Exact View Path to prevent loading wrong view
                return View("~/Views/Admin/ManageOrders.cshtml", orders);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading orders: " + ex.Message;
                return View("~/Views/Admin/ManageOrders.cshtml", new System.Collections.Generic.List<PhotoOrder>());
            }
        }

        // POST: /Admin/DeleteOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.PhotoOrders.FindAsync(id);
            if (order != null)
            {
                string folderName = $"folder_{id:D4}";
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);

                if (Directory.Exists(fullPath))
                {
                    try
                    {
                        Directory.Delete(fullPath, true);
                    }
                    catch { /* Ignore IO cleanup errors */ }
                }

                _context.PhotoOrders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Order #{id} deleted successfully!";
            }

            return RedirectToAction(nameof(ManageOrders));
        }

        // ==========================================
        // 2. MANAGE PRICES
        // ==========================================
        public async Task<IActionResult> ManagePrices()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var prices = await _context.PrintPrices.AsNoTracking().ToListAsync();
            return View(prices);
        }

        // POST: /Admin/AddPrice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPrice(PrintPrice printPrice)
        {
            if (ModelState.IsValid)
            {
                _context.PrintPrices.Add(printPrice);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Print size and price added successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to add print size. Check inputs.";
            }
            return RedirectToAction(nameof(ManagePrices));
        }

        // POST: /Admin/DeletePrice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePrice(int id)
        {
            var item = await _context.PrintPrices.FindAsync(id);
            if (item != null)
            {
                _context.PrintPrices.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Print price deleted successfully.";
            }
            return RedirectToAction(nameof(ManagePrices));
        }
    }
}