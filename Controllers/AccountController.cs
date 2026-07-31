using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DigitalPhotoPrintingSystem.Data;
using DigitalPhotoPrintingSystem.Models;

namespace DigitalPhotoPrintingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = _context.Customers.FirstOrDefault(c => c.Email == customer.Email);
                if (existingCustomer != null)
                {
                    ModelState.AddModelError("Email", "Email already exists!");
                    return View(customer);
                }

                _context.Customers.Add(customer);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter both Email and Password";
                return View();
            }

            var customer = _context.Customers.FirstOrDefault(c => c.Email == email && c.Password == password);

            if (customer != null)
            {
                HttpContext.Session.SetString("CustId", customer.CustId.ToString());
                HttpContext.Session.SetString("CustomerName", customer.F_Name);

                return RedirectToAction("Create", "PhotoOrder");
            }

            ViewBag.Error = "Invalid Email or Password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}