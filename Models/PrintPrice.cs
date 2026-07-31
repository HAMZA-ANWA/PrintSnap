using System.ComponentModel.DataAnnotations;

namespace DigitalPhotoPrintingSystem.Models
{
    public class PrintPrice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SizeName { get; set; } 

        [Required]
        public decimal Price { get; set; }
    }
}