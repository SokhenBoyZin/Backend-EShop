using Backend.DTOs.Request;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // PUBLIC
        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()

        {
            var products = await _productService.GetAllProducts();

            return Ok(products);
        }

        // PUBLIC
        // GET: api/Product/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
                return NotFound(new
                {
                    message = "Product not found"
                });

            return Ok(product);
        }

        // ADMIN ONLY
        // POST: api/Product
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            [FromBody] ProductRequest request)
        {
            var product = await _productService.CreateProduct(request);

            if (product == null)
                return NotFound("Create product failed! Category or field doesn't exists!");

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = product.ProductId },
                product
            );
        }

        // ADMIN ONLY
        // PUT: api/Product/1
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromBody] ProductRequest request)
        {
            var product = await _productService.UpdateProduct(id, request);

            if (product == null)
                return NotFound(new
                {
                    message = "Product not found"
                });

            return Ok(product);
        }

        // ADMIN ONLY
        // PATCH: api/Product/1/archive
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> ArchiveProduct(int id)
        {
            var result = await _productService.ArchiveProduct(id);

            if (!result)
                return NotFound(new
                {
                    message = "Product not found"
                });

            return Ok(new
            {
                message = "Product archived successfully"
            });
        }

        // ADMIN ONLY
        // PATCH: api/Product/1/restore
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> RestoreProduct(int id)
        {
            var result = await _productService.RestoreProduct(id);

            if (!result)
                return NotFound(new
                {
                    message = "Product not found"
                });

            return Ok(new
            {
                message = "Product restored successfully"
            });
        }

        [HttpGet("/archieved")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ArchievedProduct()
        {
            var result = await _productService.GetAllProductArchieved();

            return Ok(result);
        }

    }
}