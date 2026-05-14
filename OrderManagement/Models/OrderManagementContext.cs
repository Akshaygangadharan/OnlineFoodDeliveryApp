using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Models
{
    public class OrderManagementContext: DbContext
    {
        public OrderManagementContext(DbContextOptions<OrderManagementContext> options) : base(options)
        {
        }

        public DbSet<OrdersManagementModel> Orders { get; set; }

        //[Obsolete]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrdersManagementModel>()
                .ToTable(t => t.HasCheckConstraint("CK_Orders_Status", "[Status] IN ('Pending', 'Accepted', 'Preparing', 'Delivered')"));
        }

    }
}
