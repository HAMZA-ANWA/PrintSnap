using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace DigitalPhotoPrintingSystem.Models
{
    public class PhotoOrder
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public int OrderId
        {
            get => Id;
            set => Id = value;
        }

        public string Status { get; set; } = "Pending";

        public string PrintSize { get; set; } = string.Empty;

        public int Quantity { get; set; } = 1;

        [NotMapped]
        public IFormFile PhotoFile { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string EmailId { get; set; } = string.Empty;

        public string ShippingAddress { get; set; } = string.Empty;

        public string ModeOfPayment { get; set; } = string.Empty;

        public string FolderName { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        public string PhotoPath { get; set; } = string.Empty;

        public string EncryptedCreditCardNumber { get; set; } = string.Empty;

        public int PurchaseOrderNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public int? CustId { get; set; }

        [ForeignKey("CustId")]
        public virtual Customer Customer { get; set; }
    }
}