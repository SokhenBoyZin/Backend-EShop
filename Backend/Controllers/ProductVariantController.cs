using Backend.DTOs.Request;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantService _productVariantService;

        public ProductVariantController(
            IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductVariants()
        {
            var variants =
                await _productVariantService.GetAllProductVariants();

            return Ok(variants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductVariantById(int id)
        {
            var variant =
                await _productVariantService.GetProductVariantById(id);

            if (variant == null)
            {
                return NotFound(new
                {
                    message = "Product variant not found"
                });
            }

            return Ok(variant);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateProductVariant(
            [FromBody] ProductVariantRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request cannot be null"
                });
            }

            if (request.Price <= 0)
            {
                return BadRequest(new
                {
                    message = "Price must be greater than 0"
                });
            }

            if (request.StockQuantity < 0)
            {
                return BadRequest(new
                {
                    message = "Stock quantity cannot be negative"
                });
            }

            var variant =
                await _productVariantService
                    .CreateProductVariant(request);

            if (variant == null)
            {
                return NotFound(new
                {
                    message = "Product variant not found"
                });
            }

            return Ok(variant);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateProductVariant(
            int id,
            [FromBody] ProductVariantRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request cannot be null"
                });
            }

            if (request.Price <= 0)
            {
                return BadRequest(new
                {
                    message = "Price must be greater than 0"
                });
            }

            if (request.StockQuantity < 0)
            {
                return BadRequest(new
                {
                    message = "Stock quantity cannot be negative"
                });
            }

            var variant =
                await _productVariantService
                    .UpdateProductVariant(id, request);

            if (variant == null)
            {
                return NotFound(new
                {
                    message = "Product variant not found"
                });
            }

            return Ok(variant);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteProductVariant(int id)
        {
            var result =
                await _productVariantService
                    .DeleteProductVariant(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Product variant not found"
                });
            }

            return Ok(new
            {
                message = "Product variant deleted successfully"
            });
        }
    }
}