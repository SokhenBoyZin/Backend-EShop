using Backend.Db;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductResponse>> GetAllProducts()
        {
            var products = await _context.Products
                .Where(p => !p.IsArchived)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Capacity)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.ConnectivityType)
                .ToListAsync();

            return products.Select(MapToResponse).ToList();
        }

        public async Task<ProductResponse?> GetProductById(int id)
        {
            var product = await _context.Products
                .Where(p => p.ProductId == id && !p.IsArchived)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Capacity)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.ConnectivityType)
                .FirstOrDefaultAsync();

            if (product == null)
                return null;

            return MapToResponse(product);
        }

        public async Task<ProductResponse> CreateProduct(ProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Image = request.Image,

                ChipName = request.ChipName,
                CpuCores = request.CpuCores,
                GpuCores = request.GpuCores,
                RamGb = request.RamGb,
                DisplayName = request.DisplayName,
                DisplayResolution = request.DisplayResolution,
                MainCameraMp = request.MainCameraMp,
                FrontCameraMp = request.FrontCameraMp,
                OsVersion = request.OsVersion,

                IsArchived = false
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return MapToResponse(product);
        }

        public async Task<ProductResponse?> UpdateProduct(
            int id,
            ProductRequest request)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Capacity)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.ConnectivityType)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return null;

            product.Name = request.Name;
            product.Image = request.Image;

            product.ChipName = request.ChipName;
            product.CpuCores = request.CpuCores;
            product.GpuCores = request.GpuCores;
            product.RamGb = request.RamGb;
            product.DisplayName = request.DisplayName;
            product.DisplayResolution = request.DisplayResolution;
            product.MainCameraMp = request.MainCameraMp;
            product.FrontCameraMp = request.FrontCameraMp;
            product.OsVersion = request.OsVersion;

            await _context.SaveChangesAsync();

            return MapToResponse(product);
        }

        public async Task<bool> ArchiveProduct(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return false;

            product.IsArchived = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreProduct(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return false;

            product.IsArchived = false;

            await _context.SaveChangesAsync();

            return true;
        }

        private ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Image = product.Image,

                ChipName = product.ChipName,
                CpuCores = product.CpuCores,
                GpuCores = product.GpuCores,
                RamGb = product.RamGb,
                DisplayName = product.DisplayName,
                DisplayResolution = product.DisplayResolution,
                MainCameraMp = product.MainCameraMp,
                FrontCameraMp = product.FrontCameraMp,
                OsVersion = product.OsVersion,

                Variants = product.Variants
                    .Select(v => new ProductVariantResponse
                    {
                        ProductVariantId = v.ProductVariantId,

                        Price = v.Price,
                        StockQuantity = v.StockQuantity,

                        ColorId = v.ColorId,
                        ColorName = v.Color.Name,

                        CapacityId = v.CapacityId,
                        CapacityName = v.Capacity.SizeLabel,

                        ConnectivityTypeId = v.ConnectivityTypeId,
                        ConnectivityTypeName = v.ConnectivityType.Name
                    })
                    .ToList()
            };
        }
    }
}
