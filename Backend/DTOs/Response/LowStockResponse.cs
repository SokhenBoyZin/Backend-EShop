namespace Backend.DTOs.Product
{
    public class LowStockResponse
    {
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Image { get; set; }
        public int StockQuantity { get; set; }

        public decimal Price { get; set; }

        public string? Color { get; set; }
        public string? Capacity { get; set; }
        public string? ConnectivityType { get; set; }
    }
}