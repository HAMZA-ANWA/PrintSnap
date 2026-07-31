using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPhotoPrintingSystem.Models
{
    [Table("Prices")]
    public class Price
    {
        [Key]
        public int Id { get; set; }

        public string? PrintSize { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerPrint { get; set; }
    }
}