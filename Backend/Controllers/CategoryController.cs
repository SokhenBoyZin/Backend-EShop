using Backend.DTOs.Request;
using Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _category;

        public CategoryController(ICategoryService category)
        {
            _category = category;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _category.GetAllCategory();

            return Ok(categories);
        }

        // GET: api/Category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _category.GetCategoryById(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }

            return Ok(category);
        }

        // POST: api/Category
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request cannot be null"
                });
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new
                {
                    message = "Category name is required"
                });
            }

            var category = await _category.CreateCategory(request);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = category.CategoryId },
                category
            );
        }

        // PUT: api/Category/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] CategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request cannot be null"
                });
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new
                {
                    message = "Category name is required"
                });
            }

            var category = await _category.UpdateCategory(id, request);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }

            return Ok(category);
        }

        // DELETE: api/Category/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _category.DeleteCategory(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }

            return Ok(new
            {
                message = "Category archived successfully"
            });
        }

        // GET: api/Category/archived
        [HttpGet("archived")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllArchivedCategories()
        {
            var categories = await _category.GetArchivedCategories();

            return Ok(categories);
        }

        // PUT: api/Category/{id}/restore
        [HttpPut("{id}/restore")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RestoreCategory(int id)
        {
            var restored = await _category.RestoreCategory(id);

            if (!restored)
            {
                return NotFound(new
                {
                    message = "Archived category not found"
                });
            }

            return Ok(new
            {
                message = "Category restored successfully"
            });
        }
    }
}