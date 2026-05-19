using Microsoft.EntityFrameworkCore;

namespace OrderItems.Models
{
    public class OrderItemsContext:DbContext
    {
        public OrderItemsContext(DbContextOptions<OrderItemsContext> options) : base(options)
        {
        }
        public DbSet<OrderItemsModel> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItemsModel>()
                .ToTable(t => t.HasCheckConstraint("CK_OrderItem_Status", "[status] IN ('Pending', 'Preparing', 'Ready', 'Cancelled')"));
        }
    }
}
