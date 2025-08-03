namespace InventoryService.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
