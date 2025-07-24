using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Functions.Data;
using Functions.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Functions.Services
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        // used IServiceScopeFactory since the AddDBcontext is registered as scoped
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await CheckAndUpdateOrders();
                await Task.Delay(30000, stoppingToken); // Check every 30 seconds
            }
        }

        private async Task CheckAndUpdateOrders()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FunctionContext>();
                // Simulate checking orders (e.g., from OrderService via API or database)
                var pendingOrders = new[] { 1, 2 }; // Mock order IDs
                foreach (var orderId in pendingOrders)
                {
                    var update = new OrderStatusUpdate
                    {
                        OrderId = orderId,
                        NewStatus = "Shipped",
                        UpdateTime = DateTime.UtcNow
                    };
                    context.OrderStatusUpdates.Add(update);
                    await context.SaveChangesAsync();
                    _logger.LogInformation("Updated order {orderId} to {status} at {time}", orderId, update.NewStatus, update.UpdateTime);
                }
            }
        }
    }
}