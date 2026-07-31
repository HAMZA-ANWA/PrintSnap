using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DigitalPhotoPrintingSystem.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Email address zaroori hai")]
        [EmailAddress(ErrorMessage = "Sahi Email format enter karein")]
        [MaxLength(25, ErrorMessage = "Email 25 characters se zyada nahi ho sakti")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping Address zaroori hai")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment Mode select karein")]
        public string PaymentMode { get; set; } = string.Empty; // "CreditCard" ya "Branch"

        public string? CreditCardNumber { get; set; }

        [Required(ErrorMessage = "Print Size select karein")]
        public int PrintSizeId { get; set; }

        [Range(1, 100, ErrorMessage = "Kam se kam 1 copy required hai")]
        public int Copies { get; set; } = 1;

        public decimal TotalAmount { get; set; }

        // User ki desktop se select ki hui JPEG files yahan aayengi
        [Required(ErrorMessage = "Kam se kam ek photo upload karein")]
        public List<IFormFile>? Photos { get; set; }
    }
}