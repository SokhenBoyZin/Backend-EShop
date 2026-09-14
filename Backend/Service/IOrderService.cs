using Backend.DTOs.Order;
using Backend.Models;

namespace Backend.Services
{
    public interface IOrderService
    {
        Task<OrderResponse?> CreateOrderAsync(OrderRequest request, int userId);

        Task<List<OrderResponse>> GetMyOrdersAsync(int userId);

        Task<OrderResponse?> GetByIdAsync(int orderId, int userId);

        Task<bool> CancelOrderAsync(int orderId, int userId);

        Task<bool> UpdateStatusAsync(int orderId, OrderStatus status);

        Task<List<OrderResponse>> GetAllOrdersAsync();

        Task<OrderResponse?> AdminGetByIdAsync(int orderId);
    }
}