using Microsoft.AspNetCore.Mvc;
using InventoryService.Data;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InventoryController(InventoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventory()
        {
            var inventory = await _context.Inventory.ToListAsync();
            return Ok(inventory);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventoryItem([FromBody] InventoryItem item)
        {
            if (item == null || string.IsNullOrEmpty(item.ProductId))
            {
                return BadRequest("Product ID is required.");
            }
            _context.Inventory.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInventory), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(int id, [FromBody] InventoryItem item)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(int id)
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
    }

}