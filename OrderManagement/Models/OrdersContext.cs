using Microsoft.EntityFrameworkCore;
using Orders.Models;

namespace Orders.Models
{
    public class OrdersContext(DbContextOptions<OrdersContext> options) : DbContext(options)
    {
        public DbSet<Orders> Orders { get; set; }

        //[Obsolete]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>()
                .ToTable(t => t.HasCheckConstraint("CK_Orders_Status", "[Status] IN ('Pending', 'Confirmed', 'Preparing', 'Ready', 'Assigned', 'PickUp', 'Delivered', 'Cancelled')"));
        }

    }
}
