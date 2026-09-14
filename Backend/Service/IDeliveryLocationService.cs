using Backend.DTOs.DeliveryLocation;

namespace Backend.Services
{
    public interface IDeliveryLocationService
    {
        Task<List<DeliveryLocationResponse>> GetMyLocationsAsync(int userId);

        Task<DeliveryLocationResponse?> GetByIdAsync(int id, int userId);

        Task<DeliveryLocationResponse?> CreateAsync(DeliveryLocationRequest request, int userId);

        Task<DeliveryLocationResponse?> UpdateAsync(int id, DeliveryLocationRequest request, int userId);

        Task<bool> DeleteAsync(int id, int userId);
    }
}