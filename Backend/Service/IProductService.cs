using Backend.DTOs.Request;
using Backend.DTOs.Response;

namespace Backend.Services
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllProducts();
        Task<ProductResponse?> GetProductById(int id);

        Task<ProductResponse> CreateProduct(ProductRequest request);
        Task<ProductResponse?> UpdateProduct(int id, ProductRequest request);

        Task<bool> ArchiveProduct(int id);
        Task<bool> RestoreProduct(int id);
    }
}
