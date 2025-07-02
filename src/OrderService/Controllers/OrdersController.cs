using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public static List<Order> _orders = new List<Order>();
        private static int _nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return Ok(_orders);
        }

        [HttpPost]
        public ActionResult<Order> CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order data is required");
            }
            order.Id = _nextId++;
            order.OrderDate = DateTime.UtcNow;
            _orders.Add(order);
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);

        }
    }
}
