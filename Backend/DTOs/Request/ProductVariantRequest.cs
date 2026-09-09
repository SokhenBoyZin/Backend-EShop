namespace Backend.DTOs.Request
{
    public class ProductVariantRequest
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int ColorId { get; set; }
        public int CapacityId { get; set; }
        public int ConnectivityTypeId { get; set; }
    }
}
