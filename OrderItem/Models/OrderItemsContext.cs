using Microsoft.EntityFrameworkCore;

namespace OrderItems.Models
{
    public class OrderItemsContext:DbContext
    {
        public OrderItemsContext(DbContextOptions<OrderItemsContext> options) : base(options)
        {
        }
        public DbSet<OrderItemsModel> OrderItems { get; set; }

        
    }
}
