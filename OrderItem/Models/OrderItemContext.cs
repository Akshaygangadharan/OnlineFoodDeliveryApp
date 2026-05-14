using Microsoft.EntityFrameworkCore;

namespace OrderItem.Models
{
    public class OrderItemContext:DbContext
    {
        public OrderItemContext(DbContextOptions<OrderItemContext> options) : base(options)
        {
        }
        public DbSet<OrderItemModel> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItemModel>()
                .ToTable(t => t.HasCheckConstraint("CK_OrderItem_Status", "[status] IN ('cart', 'ordered')"));
        }
    }
}
