using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalPhotoPrintingSystem.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        public string PaymentMode { get; set; } = string.Empty;

        public string? EncryptedCreditCard { get; set; }

        public string? FolderName { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}