using Backend.DTOs.Request;
using Backend.DTOs.Response;

namespace Backend.Services
{
    public interface IConnectivityTypeService
    {
        Task<List<ConnectivityTypeResponse>> GetAllConnectivityTypes();
        Task<ConnectivityTypeResponse?> GetConnectivityTypeById(int id);
        Task<ConnectivityTypeResponse> CreateConnectivityType(
            ConnectivityTypeRequest request);
        Task<ConnectivityTypeResponse?> UpdateConnectivityType(
            int id,
            ConnectivityTypeRequest request);
        Task<bool> DeleteConnectivityType(int id);
    }
}