using Microsoft.EntityFrameworkCore;
using DigitalPhotoPrintingSystem.Models;

namespace DigitalPhotoPrintingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PrintPrice> PrintPrices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PhotoOrder> PhotoOrders { get; set; }
    }
}