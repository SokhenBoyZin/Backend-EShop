using Backend.DTOs.Request;
using Backend.DTOs.Response;

namespace Backend.Services
{
    public interface IColorService
    {
        Task<List<ColorResponse>> GetAllColors();
        Task<ColorResponse?> GetColorById(int id);
        Task<ColorResponse> CreateColor(ColorRequest request);
        Task<ColorResponse?> UpdateColor(int id, ColorRequest request);
        Task<bool> DeleteColor(int id);
    }
}