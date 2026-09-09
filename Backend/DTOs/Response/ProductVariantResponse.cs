namespace Backend.DTOs.Response
{
    public class ProductVariantResponse
    {
        public int ProductVariantId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; } = string.Empty;
        public int CapacityId { get; set; }
        public string CapacityName { get; set; } = string.Empty;
        public int ConnectivityTypeId { get; set; }
        public string ConnectivityTypeName { get; set; } = string.Empty;
    }
}
