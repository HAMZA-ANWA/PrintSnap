using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalPhotoPrintingSystem.Models
{
    public class Customer
    {
        [Key]
        public int CustId { get; set; }

        public string F_Name { get; set; } = string.Empty;

        public string L_Name { get; set; } = string.Empty;

        public DateTime Dob { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string P_No { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}