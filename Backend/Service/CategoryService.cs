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

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET ALL ACTIVE CATEGORIES
        public async Task<List<CategoryResponse>> GetAllCategory()
        {
            var categories = await _context.Categories
                .Where(c => !c.IsArchived)
                .Select(c => new CategoryResponse
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,

                    // Count only active products
                    ProductsCount = c.Products
                        .Count(p => !p.IsArchived)
                })
                .ToListAsync();

            return categories;
        }


        // GET ALL ARCHIVED CATEGORIES
        public async Task<List<CategoryResponse>> GetArchivedCategories()
        {
            var categories = await _context.Categories
                .Where(c => c.IsArchived)
                .Select(c => new CategoryResponse
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,

                    // Count only active products
                    ProductsCount = c.Products
                        .Count(p => !p.IsArchived)
                })
                .ToListAsync();

            return categories;
        }


        // GET CATEGORY BY ID
        public async Task<CategoryResponse?> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryId == id && !c.IsArchived)
                .Select(c => new CategoryResponse
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,

                    ProductsCount = c.Products
                        .Count(p => !p.IsArchived)
                })
                .FirstOrDefaultAsync();

            return category;
        }


        // CREATE CATEGORY
        public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name,
                IsArchived = false
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ProductsCount = 0
            };
        }


        // UPDATE CATEGORY
        public async Task<CategoryResponse?> UpdateCategory(
            int id,
            CategoryRequest request)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return null;

            category.Name = request.Name;

            await _context.SaveChangesAsync();

            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                Name = category.Name,

                ProductsCount = await _context.Products
                    .CountAsync(p =>
                        p.CategoryId == category.CategoryId &&
                        !p.IsArchived)
            };
        }


        // SOFT DELETE CATEGORY
        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return false;

            category.IsArchived = true;

            await _context.SaveChangesAsync();

            return true;
        }


        // RESTORE CATEGORY
        public async Task<bool> RestoreCategory(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return false;

            category.IsArchived = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}