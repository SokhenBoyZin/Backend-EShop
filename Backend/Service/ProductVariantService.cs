using Backend.Db;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductVariantResponse>> GetAllProductVariants()
        {
            var variants = await _context.ProductVariants
                .Include(v => v.Color)
                .Include(v => v.Capacity)
                .Include(v => v.ConnectivityType)
                .ToListAsync();

            return variants
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<ProductVariantResponse?> GetProductVariantById(int id)
        {
            var variant = await _context.ProductVariants
                .Include(v => v.Color)
                .Include(v => v.Capacity)
                .Include(v => v.ConnectivityType)
                .FirstOrDefaultAsync(v => v.ProductVariantId == id);

            if (variant == null)
                return null;

            return MapToResponse(variant);
        }

        // CREATE
        public async Task<ProductVariantResponse?> CreateProductVariant(
            ProductVariantRequest request)
        {
            // Check Product
            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductId == request.ProductId &&
                    !p.IsArchived);

            if (product == null)
                return null;

            // Check Color
            var color = await _context.Colors
                .FirstOrDefaultAsync(c =>
                    c.ColorId == request.ColorId);

            if (color == null)
                return null;

            // Check Capacity
            var capacity = await _context.Capacities
                .FirstOrDefaultAsync(c =>
                    c.CapacityId == request.CapacityId);

            if (capacity == null)
                return null;

            // Check ConnectivityType
            var connectivityType = await _context.ConnectivityTypes
                .FirstOrDefaultAsync(c =>
                    c.ConnectivityTypeId == request.ConnectivityTypeId);

            if (connectivityType == null)
                return null;

            var variant = new ProductVariant
            {
                ProductId = request.ProductId,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                ColorId = request.ColorId,
                CapacityId = request.CapacityId,
                ConnectivityTypeId = request.ConnectivityTypeId
            };

            _context.ProductVariants.Add(variant);

            await _context.SaveChangesAsync();

            // Load relationships for response
            await _context.Entry(variant)
                .Reference(v => v.Color)
                .LoadAsync();

            await _context.Entry(variant)
                .Reference(v => v.Capacity)
                .LoadAsync();

            await _context.Entry(variant)
                .Reference(v => v.ConnectivityType)
                .LoadAsync();

            return MapToResponse(variant);
        }

        public async Task<ProductVariantResponse?> UpdateProductVariant(
            int id,
            ProductVariantRequest request)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v =>
                    v.ProductVariantId == id);

            if (variant == null)
                return null;

            // Check Product
            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductId == request.ProductId &&
                    !p.IsArchived);

            if (product == null)
                return null;

            // Check Color
            var color = await _context.Colors
                .FirstOrDefaultAsync(c =>
                    c.ColorId == request.ColorId);

            if (color == null)
                return null;

            // Check Capacity
            var capacity = await _context.Capacities
                .FirstOrDefaultAsync(c =>
                    c.CapacityId == request.CapacityId);

            if (capacity == null)
                return null;

            // Check ConnectivityType
            var connectivityType = await _context.ConnectivityTypes
                .FirstOrDefaultAsync(c =>
                    c.ConnectivityTypeId == request.ConnectivityTypeId);

            if (connectivityType == null)
                return null;

            variant.ProductId = request.ProductId;
            variant.Price = request.Price;
            variant.StockQuantity = request.StockQuantity;
            variant.ColorId = request.ColorId;
            variant.CapacityId = request.CapacityId;
            variant.ConnectivityTypeId = request.ConnectivityTypeId;

            await _context.SaveChangesAsync();

            // Load relationships
            await _context.Entry(variant)
                .Reference(v => v.Color)
                .LoadAsync();

            await _context.Entry(variant)
                .Reference(v => v.Capacity)
                .LoadAsync();

            await _context.Entry(variant)
                .Reference(v => v.ConnectivityType)
                .LoadAsync();

            return MapToResponse(variant);
        }

        public async Task<bool> DeleteProductVariant(int id)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v =>
                    v.ProductVariantId == id);

            if (variant == null)
                return false;

            _context.ProductVariants.Remove(variant);

            await _context.SaveChangesAsync();

            return true;
        }

        private ProductVariantResponse MapToResponse(
            ProductVariant variant)
        {
            return new ProductVariantResponse
            {
                ProductVariantId = variant.ProductVariantId,
                Price = variant.Price,
                StockQuantity = variant.StockQuantity,

                ColorId = variant.ColorId,
                ColorName = variant.Color.Name,

                CapacityId = variant.CapacityId,
                CapacityName = variant.Capacity.SizeLabel,

                ConnectivityTypeId = variant.ConnectivityTypeId,
                ConnectivityTypeName = variant.ConnectivityType.Name
            };
        }
    }
}