namespace Backend.DTOs.Order
{
    public class OrderItemResponse
    {
        public int OrderItemId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Capacity { get; set; } = string.Empty;
        public string Connectivity { get; set; } = string.Empty;
    }
}
