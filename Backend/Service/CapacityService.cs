using Backend.Db;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class CapacityService : ICapacityService
    {
        private readonly ApplicationDbContext _context;

        public CapacityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CapacityResponse>> GetAllCapacities()
        {
            var capacities = await _context.Capacities
                .ToListAsync();

            return capacities
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<CapacityResponse?> GetCapacityById(int id)
        {
            var capacity = await _context.Capacities
                .FirstOrDefaultAsync(c => c.CapacityId == id);

            if (capacity == null)
                return null;

            return MapToResponse(capacity);
        }

        public async Task<CapacityResponse> CreateCapacity(CapacityRequest request)
        {
            var capacity = new Capacity
            {
                SizeLabel = request.SizeLabel
            };

            _context.Capacities.Add(capacity);

            await _context.SaveChangesAsync();

            return MapToResponse(capacity);
        }

        public async Task<CapacityResponse?> UpdateCapacity(int id,CapacityRequest request)
        {
            var capacity = await _context.Capacities
                .FirstOrDefaultAsync(c => c.CapacityId == id);

            if (capacity == null)
                return null;

            capacity.SizeLabel = request.SizeLabel;

            await _context.SaveChangesAsync();

            return MapToResponse(capacity);
        }

        public async Task<bool> DeleteCapacity(int id)
        {
            var capacity = await _context.Capacities
                .FirstOrDefaultAsync(c => c.CapacityId == id);

            if (capacity == null)
                return false;

            _context.Capacities.Remove(capacity);

            await _context.SaveChangesAsync();

            return true;
        }

        private CapacityResponse MapToResponse(Capacity capacity)
        {
            return new CapacityResponse
            {
                CapacityId = capacity.CapacityId,
                SizeLabel = capacity.SizeLabel
            };
        }
    }
}