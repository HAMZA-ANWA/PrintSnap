using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPhotoPrintingSystem.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }

        public int? CustId { get; set; }

        [ForeignKey("CustId")]
        public Customer? Customer { get; set; }

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