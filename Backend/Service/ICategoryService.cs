using Backend.DTOs.Request;
using Backend.DTOs.Response;

namespace Backend.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllCategory();

        Task<List<CategoryResponse>> GetArchivedCategories();

        Task<CategoryResponse?> GetCategoryById(int id);

        Task<CategoryResponse> CreateCategory(CategoryRequest request);

        Task<CategoryResponse?> UpdateCategory(
            int id,
            CategoryRequest request);

        Task<bool> DeleteCategory(int id);

        Task<bool> RestoreCategory(int id);
    }
}