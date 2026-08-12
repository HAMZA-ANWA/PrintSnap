using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DigitalPhotoPrintingSystem.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public string CustomerEmail { get; set; }

        public int PrintSizeId { get; set; }

        public int Copies { get; set; } = 1;

        public string PaymentMode { get; set; }

        public string? CreditCardNumber { get; set; }

        public string ShippingAddress { get; set; }

        // Multiple Photo files ke liye (Form upload handle karne ke liye)
        public List<IFormFile>? Photos { get; set; }

        // Calculated Total Cost
        public decimal TotalCost { get; set; }

        // Order Creation Date
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}