using Backend.Db;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ColorService : IColorService
    {
        private readonly ApplicationDbContext _context;

        public ColorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ColorResponse>> GetAllColors()
        {
            var colors = await _context.Colors
                .ToListAsync();

            return colors.Select(MapToResponse).ToList();
        }

        public async Task<ColorResponse?> GetColorById(int id)
        {
            var color = await _context.Colors
                .FirstOrDefaultAsync(c => c.ColorId == id);

            if (color == null)
                return null;

            return MapToResponse(color);
        }

        public async Task<ColorResponse> CreateColor(ColorRequest request)
        {
            var color = new Color
            {
                Name = request.Name
            };

            _context.Colors.Add(color);
            await _context.SaveChangesAsync();

            return MapToResponse(color);
        }

        public async Task<ColorResponse?> UpdateColor(int id, ColorRequest request)
        {
            var color = await _context.Colors
                .FirstOrDefaultAsync(c => c.ColorId == id);
            if (request.Name == null) 
            {
                return null;           
            }
            if (color == null)
                return null;

            color.Name = request.Name;

            await _context.SaveChangesAsync();

            return MapToResponse(color);
        }

        public async Task<bool> DeleteColor(int id)
        {
            var color = await _context.Colors
                .FirstOrDefaultAsync(c => c.ColorId == id);

            if (color == null)
                return false;

            _context.Colors.Remove(color);

            await _context.SaveChangesAsync();

            return true;
        }

        private ColorResponse MapToResponse(Color color)
        {
            return new ColorResponse
            {
                ColorId = color.ColorId,
                ColorName = color.Name
            };
        }
    }
}