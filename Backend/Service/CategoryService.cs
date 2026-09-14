using Backend.Db;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext _con)
        {
            _context = _con;
        }

        public async Task<List<CategoryResponse>> GetAllCategory()
        {
            var category = await _context.Categories.ToListAsync();

            return category
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<CategoryResponse?> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return null;

            return MapToResponse(category);
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return MapToResponse(category);
        }

        public async Task<CategoryResponse?> UpdateCategory(int id, CategoryRequest request)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return null;

            category.Name = request.Name;

            await _context.SaveChangesAsync();

            return MapToResponse(category);
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return false;

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return true;
        }

        private CategoryResponse MapToResponse(Category category)
        {
            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
            };
        }
    }
}
