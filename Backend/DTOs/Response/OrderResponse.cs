using Backend.DTOs.DeliveryLocation;
using Backend.Models;

namespace Backend.DTOs.Order
{
    public class OrderResponse
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DeliveryLocationResponse DeliveryLocation { get; set; } = null!;

        public decimal Subtotal { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public decimal DeliveryFee { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
    }
}
