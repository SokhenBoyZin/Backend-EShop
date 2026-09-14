using Backend.Models;
namespace Backend.DTOs.Order
{
    public class OrderRequest
    {
        public int DeliveryLocationId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public List<OrderItemRequest> Items { get; set; }
            = new List<OrderItemRequest>();
    }
}
