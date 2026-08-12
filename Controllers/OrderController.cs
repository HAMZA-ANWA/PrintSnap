using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigitalPhotoPrintingSystem.Data;
using DigitalPhotoPrintingSystem.Models;

namespace DigitalPhotoPrintingSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Price Calculation
                decimal pricePerPrint = model.PrintSizeId switch
                {
                    1 => 10.00m,
                    2 => 15.00m,
                    3 => 25.00m,
                    4 => 35.00m,
                    _ => 10.00m
                };

                int photoCount = (model.Photos != null && model.Photos.Count > 0) ? model.Photos.Count : 1;
                decimal calculatedTotal = pricePerPrint * model.Copies * photoCount;

                string email = !string.IsNullOrEmpty(model.CustomerEmail) ? model.CustomerEmail : "customer@gmail.com";
                string name = email.Contains("@") ? email.Split('@')[0] : "Customer";

                // 1. Database Entity Object Create
                var newOrder = new PhotoOrder
                {
                    EmailId = email,
                    CustomerName = name,
                    ModeOfPayment = !string.IsNullOrEmpty(model.PaymentMode) ? model.PaymentMode : "Direct Payment in Branch",
                    ShippingAddress = model.ShippingAddress ?? "",
                    EncryptedCreditCardNumber = model.CreditCardNumber ?? "",
                    PrintSize = $"Size ID: {model.PrintSizeId}",
                    Quantity = model.Copies,
                    UnitPrice = pricePerPrint,
                    TotalPrice = calculatedTotal,
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                };

                // 2. Save to Database
                _context.PhotoOrders.Add(newOrder);
                await _context.SaveChangesAsync();

                // 3. Save Uploaded Photos (If Any)
                if (model.Photos != null && model.Photos.Count > 0)
                {
                    string folderName = $"folder_{newOrder.Id:D4}";
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var photo in model.Photos)
                    {
                        if (photo.Length > 0)
                        {
                            string filePath = Path.Combine(uploadsFolder, photo.FileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await photo.CopyToAsync(stream);
                            }
                        }
                    }

                    newOrder.FolderName = folderName;
                    await _context.SaveChangesAsync();
                }

                // 4. Update Model for Receipt View
                model.OrderId = newOrder.Id;
                model.TotalCost = calculatedTotal;
                model.CreatedDate = newOrder.OrderDate;

                return View("OrderSuccess", model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult OrderSuccess(OrderViewModel model)
        {
            return View(model);
        }
    }
}