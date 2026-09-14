using Backend.DTOs.Request;
using Backend.Models;
using Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var category = await _category.GetAllCategory();

            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _category.GetCategoryById(id);

            if (category == null)
            {
                return NotFound("Category id: " + id + " not found!");
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateCategory(CategoryRequest request)
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
                new { id = category.CategoryId }, category
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequest request)
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
                   message = "Please fill all the field!"
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _category.DeleteCategory(id);

            if (!category)
            {
                return BadRequest(new
                {
                    message = "Category not found!"
                });
            }

            return Ok(new
            {
                message = "Category deleted successfully"
            });
        }
    }
}
