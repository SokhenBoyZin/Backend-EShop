namespace Backend.DTOs.Order
{
    public class OrderItemRequest
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
