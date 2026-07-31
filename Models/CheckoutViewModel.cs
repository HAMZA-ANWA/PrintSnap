using System.ComponentModel.DataAnnotations;

namespace DigitalPhotoPrintingSystem.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(25, ErrorMessage = "Email cannot exceed 25 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping Address is required")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a payment method")]
        public string PaymentMethod { get; set; } = "Direct"; // Direct or CreditCard

        // Credit Card Details
        [StringLength(20, ErrorMessage = "Card number cannot exceed 20 digits")]
        public string? CardNumber { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }

        public decimal TotalAmount { get; set; }
    }
}