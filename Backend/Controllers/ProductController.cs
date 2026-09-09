using Backend.DTOs.Request;
using Backend.Services;
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


        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();

            return Ok(products);
        }


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


        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            [FromBody] ProductRequest request)
        {
            var product = await _productService.CreateProduct(request);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = product.ProductId },
                product
            );
        }


        // PUT: api/Product/1
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


        // PATCH: api/Product/1/archive
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


        // PATCH: api/Product/1/restore
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
    }
}
