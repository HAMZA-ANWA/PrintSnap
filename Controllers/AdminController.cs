using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalPhotoPrintingSystem.Data;
using DigitalPhotoPrintingSystem.Models;

namespace DigitalPhotoPrintingSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> ManageOrders()
        {
            try
            {
                var orders = await _context.PurchaseOrders.ToListAsync();
                return View(orders ?? new List<PurchaseOrder>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Database Error: " + ex.Message;
                return View(new List<PurchaseOrder>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteAndCleanupFolder(int id)
        {
            try
            {
                var order = await _context.PurchaseOrders.FindAsync(id);
                if (order != null)
                {
                    if (!string.IsNullOrEmpty(order.FolderName))
                    {
                        string folderPath = Path.Combine(_environment.WebRootPath, "uploads", order.FolderName);
                        if (Directory.Exists(folderPath))
                        {
                            Directory.Delete(folderPath, true);
                        }
                    }

                    _context.PurchaseOrders.Remove(order);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Order #{id} completed and server photos deleted successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error while processing order: " + ex.Message;
            }

            return RedirectToAction("ManageOrders");
        }

        public async Task<IActionResult> ManagePrices()
        {
            try
            {
                var prices = await _context.PrintPrices.ToListAsync();
                return View(prices ?? new List<PrintPrice>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading prices: " + ex.Message;
                return View(new List<PrintPrice>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPrice(PrintPrice model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.PrintPrices.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "New print size added successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error adding price: " + ex.Message;
            }
            return RedirectToAction("ManagePrices");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrice(int id)
        {
            try
            {
                var price = await _context.PrintPrices.FindAsync(id);
                if (price != null)
                {
                    _context.PrintPrices.Remove(price);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Print size removed successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting price: " + ex.Message;
            }
            return RedirectToAction("ManagePrices");
        }
    }
}