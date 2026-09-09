using Backend.DTOs.Request;
using Backend.DTOs.Response;

namespace Backend.Services
{
    public interface IProductVariantService
    {
        Task<List<ProductVariantResponse>> GetAllProductVariants();
        Task<ProductVariantResponse?> GetProductVariantById(int id);
        Task<ProductVariantResponse?> CreateProductVariant(ProductVariantRequest request);
        Task<ProductVariantResponse?> UpdateProductVariant(int id, ProductVariantRequest request);
        Task<bool> DeleteProductVariant(int id);
    }
}