using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Functions.Data;
using Functions.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text.Json;


using OrderService.Models;

namespace Functions.Services
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        // used IServiceScopeFactory since the AddDBcontext is registered as scoped
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory , IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44361/gateway/");
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
                // Simulate checking orders (e.g., from OrderService via API or database)
                var context = scope.ServiceProvider.GetRequiredService<FunctionContext>();
                var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVGVzdFVzZXIiLCJleHAiOjE3NTQwNzY3NjcsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzYxIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNjEifQ.2HqDA_b7aGmHG3B06I3yxRVnzVoDHSie1WJnKjOSAJc"; // Replace with actual token retrieval logic
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("Orders?status=Pending");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pendingOrders = JsonSerializer.Deserialize<List<Order>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    foreach (var order in pendingOrders ?? new List<Order>())
                    {
                        var update = new OrderStatusUpdate
                        {
                            OrderId = order.Id,
                            NewStatus = "Shipped",
                            UpdateTime = DateTime.UtcNow
                        };
                        context.OrderStatusUpdates.Add(update);
                        await context.SaveChangesAsync();
                        _logger.LogInformation("Updated order {orderId} to {status} at {time}", order.Id, update.NewStatus, update.UpdateTime);
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to fetch pending orders: {StatusCode}", response.StatusCode);
                }

            }
        }
    }
}