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
        public IActionResult Register(
            string FirstName,
            string LastName,
            DateTime DateOfBirth,
            string Gender,
            string PhoneNumber,
            string Address,
            string Email,
            string Password)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError("", "Email and Password are required.");
                return View();
            }

            string cleanEmail = Email.Trim().ToLower();

            var existingCustomer = _context.Customers.FirstOrDefault(c => c.Email.ToLower() == cleanEmail);
            if (existingCustomer != null)
            {
                ModelState.AddModelError("", "Email already exists! Please login.");
                return View();
            }

            var customer = new Customer
            {
                F_Name = FirstName ?? "",
                L_Name = LastName ?? "",
                Dob = DateOfBirth,
                Gender = Gender ?? "",
                P_No = PhoneNumber ?? "",
                Address = Address ?? "",
                Email = cleanEmail,
                Password = Password.Trim()
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            // Set Sessions
            string displayName = !string.IsNullOrWhiteSpace(customer.F_Name)
                                 ? customer.F_Name.Trim()
                                 : customer.Email.Split('@')[0];

            HttpContext.Session.SetString("CustId", customer.CustId.ToString());
            HttpContext.Session.SetString("UserName", displayName);
            HttpContext.Session.SetString("CustomerEmail", customer.Email ?? "");

            TempData["Success"] = "Account created successfully!";
            return RedirectToAction("Index", "Home");
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

            string cleanEmail = email.Trim().ToLower();
            string cleanPassword = password.Trim();

            var customer = _context.Customers.FirstOrDefault(c => c.Email.ToLower() == cleanEmail && c.Password == cleanPassword);

            if (customer != null)
            {
                string displayName = !string.IsNullOrWhiteSpace(customer.F_Name)
                                     ? customer.F_Name.Trim()
                                     : customer.Email.Split('@')[0];

                HttpContext.Session.SetString("CustId", customer.CustId.ToString());
                HttpContext.Session.SetString("UserName", displayName);
                HttpContext.Session.SetString("CustomerEmail", customer.Email ?? "");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid Email or Password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}