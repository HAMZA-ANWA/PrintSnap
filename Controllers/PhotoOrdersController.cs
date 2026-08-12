using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigitalPhotoPrintingSystem.Data;
using DigitalPhotoPrintingSystem.Models;

namespace DigitalPhotoPrintingSystem.Controllers
{
    public class PhotoOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhotoOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("ManageOrders", "Admin");
        }

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
                try
                {
                    decimal unitPrice = 0;
                    string sizeName = "";

                    switch (model.PrintSizeId)
                    {
                        case 1: unitPrice = 10m; sizeName = "4x6"; break;
                        case 2: unitPrice = 15m; sizeName = "5x7"; break;
                        case 3: unitPrice = 25m; sizeName = "8x10"; break;
                        case 4: unitPrice = 35m; sizeName = "A4"; break;
                        default: unitPrice = 10m; sizeName = "Standard"; break;
                    }

                    int photoCount = (model.Photos != null && model.Photos.Count > 0) ? model.Photos.Count : 1;
                    decimal totalPrice = unitPrice * model.Copies * photoCount;

                    string customerEmail = !string.IsNullOrEmpty(model.CustomerEmail) ? model.CustomerEmail : "customer@example.com";
                    string customerName = customerEmail.Contains("@") ? customerEmail.Split('@')[0] : "Customer";

                    var photoOrder = new PhotoOrder
                    {
                        EmailId = customerEmail,
                        CustomerName = customerName,
                        ModeOfPayment = model.PaymentMode,
                        ShippingAddress = model.ShippingAddress,
                        EncryptedCreditCardNumber = model.CreditCardNumber ?? "",
                        PrintSize = sizeName,
                        Quantity = model.Copies,
                        UnitPrice = unitPrice,
                        TotalPrice = totalPrice,
                        OrderDate = DateTime.Now,
                        Status = "Pending"
                    };

                    _context.PhotoOrders.Add(photoOrder);
                    await _context.SaveChangesAsync();

                    if (model.Photos != null && model.Photos.Count > 0)
                    {
                        string folderName = $"folder_{photoOrder.Id:D4}";
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

                        photoOrder.FolderName = folderName;
                        await _context.SaveChangesAsync();
                    }

                    // Assign updated values for receipt rendering
                    model.OrderId = photoOrder.Id;
                    model.TotalCost = totalPrice;
                    model.CreatedDate = photoOrder.OrderDate;

                    return View("~/Views/PhotoOrders/Confirmation.cshtml", model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving order: " + ex.Message);
                }
            }

            return View(model);
        }
    }
}