using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrdersController(OrderContext context)
        {
            _context = context;
        }

        // <summary>
        /// Retrieves a list of all orders.
        /// </summary>
        /// <returns>A list of orders.</returns>
        /// <response code="200">Returns the list of orders.</response>
        /// <response code="500">If there is an internal server error.</response>

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            try
            {
                if (order == null || string.IsNullOrEmpty(order.UserId))
                {
                    return BadRequest("User ID is required.");
                }
                order.OrderDate = DateTime.UtcNow;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating order: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            try
            {
                if (order == null || id != order.Id)
                {
                    return BadRequest("Invalid ID or order data.");
                }
                var existingOrder = await _context.Orders.FindAsync(id);
                if (existingOrder == null)
                {
                    return NotFound();
                }
                existingOrder.UserId = order.UserId;
                existingOrder.ProductId = order.ProductId;
                existingOrder.Quantity = order.Quantity;
                existingOrder.Status = order.Status;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating order: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting order: {ex.Message}");
            }
        }
    }
}