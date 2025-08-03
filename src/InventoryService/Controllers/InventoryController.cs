using Microsoft.AspNetCore.Mvc;
using InventoryService.Data;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InventoryController(InventoryContext context)
        {
            _context = context;
        }
        // <summary>
        /// Retrieves a list of all Inventories.
        /// </summary>
        /// <returns>A list of orders.</returns>
        /// <response code="200">Returns the list of Inventories.</response>
        /// <response code="500">If there is an internal server error.</response>

        [HttpGet]
        public async Task<IActionResult> GetInventory()
        {
            try
            {
                var inventory = await _context.Inventory.ToListAsync();
                return Ok(inventory);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventoryItem([FromBody] InventoryItem item)
        {
            try
            {
                if (item == null || string.IsNullOrEmpty(item.ProductId))
                {
                    return BadRequest("Product ID is required.");
                }
                _context.Inventory.Add(item);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetInventory), new { id = item.Id }, item);
            }

            catch(Exception ex)
            {
                return StatusCode(500, $"Error Creating Inventory: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(int id, [FromBody] InventoryItem item)
        {
            try
            {
                if (item == null || id != item.Id)
                {
                    return BadRequest("Invalid ID or inventory data.");
                }
                var existingItem = await _context.Inventory.FindAsync(id);
                if (existingItem == null)
                {
                    return NotFound();
                }
                existingItem.ProductId = item.ProductId;
                existingItem.Name = item.Name;
                existingItem.Stock = item.Stock;
                existingItem.Price = item.Price;
                await _context.SaveChangesAsync();
                return NoContent();
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Error Updating Inventory: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(int id)
        {
            try
            {
                var item = await _context.Inventory.FindAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                _context.Inventory.Remove(item);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Error Deleting Inventory: {ex.Message}");
            }
        }
    }

}