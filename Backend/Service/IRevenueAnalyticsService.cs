using Backend.DTOs.Analytics;

namespace Backend.Services.Interfaces
{
    public interface IRevenueAnalyticsService
    {
        Task<RevenueResponse> GetRevenueAsync(RevenueRequest request);
    }
}