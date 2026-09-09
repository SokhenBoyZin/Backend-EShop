using Backend.DTOs.Request;
using Backend.DTOs.Response;

namespace Backend.Services
{
    public interface ICapacityService
    {
        Task<List<CapacityResponse>> GetAllCapacities();
        Task<CapacityResponse?> GetCapacityById(int id);
        Task<CapacityResponse> CreateCapacity(CapacityRequest request);
        Task<CapacityResponse?> UpdateCapacity(int id, CapacityRequest request);
        Task<bool> DeleteCapacity(int id);
    }
}