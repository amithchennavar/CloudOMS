using Microsoft.EntityFrameworkCore;
using OrderService.Models;
//Order Context
namespace OrderService.Data
{
    public class OrderContext :DbContext
    {
        public virtual DbSet<Order> Orders { get; set; }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=orders.db");
            }
        }
    }
}
