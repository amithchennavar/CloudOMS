using InventoryService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private static List<InventoryItem> _inventory = new List<InventoryItem>();
        private static int _nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<InventoryItem>> GetInventory()
        {
            return Ok(_inventory);
        }

        [HttpPost]
        public ActionResult<InventoryItem> CreateInventoryItem([FromBody] InventoryItem item)
        {
            if (item == null || string.IsNullOrEmpty(item.ProductId))
            {
                return BadRequest("Product ID is required.");
            }
            item.Id = _nextId++;
            _inventory.Add(item);
            return CreatedAtAction(nameof(GetInventory), new { id = item.Id }, item);
        }
    }
}
