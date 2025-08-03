using Functions.Models;
using Microsoft.EntityFrameworkCore;
namespace Functions.Data
{
    public class FunctionContext : DbContext
    {
        public DbSet<OrderStatusUpdate> OrderStatusUpdates { get; set; }

        public FunctionContext(DbContextOptions<FunctionContext> options) : base(options)
        {
        }
    }
}