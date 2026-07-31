using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalPhotoPrintingSystem.Data;
using DigitalPhotoPrintingSystem.Models;

namespace DigitalPhotoPrintingSystem.Controllers
{
    public class PhotoOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PhotoOrderController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: PhotoOrder/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var printSizes = await _context.PrintPrices.ToListAsync();
            ViewBag.PrintSizes = new SelectList(printSizes, "Id", "SizeName");
            return View();
        }

        // POST: PhotoOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Price calculation logic
                var selectedSize = await _context.PrintPrices.FirstOrDefaultAsync(p => p.Id == model.PrintSizeId);
                decimal pricePerCopy = selectedSize != null ? selectedSize.Price : 0;
                int photoCount = model.Photos != null ? model.Photos.Count : 1;
                decimal calculatedTotal = pricePerCopy * model.Copies * photoCount;

                var order = new PurchaseOrder
                {
                    CustomerEmail = model.CustomerEmail,
                    ShippingAddress = model.ShippingAddress,
                    PaymentMode = model.PaymentMode,
                    OrderDate = DateTime.Now,
                    TotalAmount = calculatedTotal
                };

                if (model.PaymentMode == "CreditCard" && !string.IsNullOrEmpty(model.CreditCardNumber))
                {
                    order.EncryptedCreditCard = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.CreditCardNumber));
                }

                _context.PurchaseOrders.Add(order);
                await _context.SaveChangesAsync();

                // Create folder_xxxx directory
                string folderName = $"folder_{order.Id:D4}";
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folderName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Upload JPEG files
                if (model.Photos != null)
                {
                    foreach (var file in model.Photos)
                    {
                        var ext = Path.GetExtension(file.FileName).ToLower();
                        if (ext == ".jpg" || ext == ".jpeg")
                        {
                            string filePath = Path.Combine(uploadsFolder, file.FileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                    }
                }

                order.FolderName = folderName;
                await _context.SaveChangesAsync();

                return RedirectToAction("OrderConfirmation", new { id = order.Id });
            }

            var sizes = await _context.PrintPrices.ToListAsync();
            ViewBag.PrintSizes = new SelectList(sizes, "Id", "SizeName");
            return View(model);
        }

        // GET: PhotoOrder/OrderConfirmation/1
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.PurchaseOrders.FindAsync(id);
            return View(order);
        }

        // GET: PhotoOrder/ManageOrders (Requirement #9)
        public async Task<IActionResult> ManageOrders()
        {
            var orders = await _context.PurchaseOrders.ToListAsync();
            return View(orders);
        }

        // POST: PhotoOrder/ExecuteOrder/1 (Requirement #10)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExecuteOrder(int id)
        {
            var order = await _context.PurchaseOrders.FindAsync(id);
            if (order != null)
            {
                // Delete folder containing photographs from server
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
            }

            return RedirectToAction(nameof(ManageOrders));
        }
    }
}