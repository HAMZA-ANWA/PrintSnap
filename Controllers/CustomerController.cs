using System;
using Microsoft.AspNetCore.Mvc;
using DigitalPhotoPrintingSystem.Data;
using DigitalPhotoPrintingSystem.Models;

namespace DigitalPhotoPrintingSystem.Controllers
{
	public class CustomerController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CustomerController(ApplicationDbContext context)
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
			try
			{
				_context.Customers.Add(customer);
				_context.SaveChanges();

				return Content("Customer Registered Successfully");
			}
			catch (Exception ex)
			{
				return Content(ex.InnerException?.Message ?? ex.Message);
			}
		}
	}
}