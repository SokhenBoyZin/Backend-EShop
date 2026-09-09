using Backend.Db;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ConnectivityTypeService : IConnectivityTypeService
    {
        private readonly ApplicationDbContext _context;

        public ConnectivityTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<List<ConnectivityTypeResponse>>
            GetAllConnectivityTypes()
        {
            var connectivityTypes =
                await _context.ConnectivityTypes.ToListAsync();

            return connectivityTypes
                .Select(MapToResponse)
                .ToList();
        }

        // GET BY ID
        public async Task<ConnectivityTypeResponse?>
            GetConnectivityTypeById(int id)
        {
            var connectivityType =
                await _context.ConnectivityTypes
                    .FirstOrDefaultAsync(
                        c => c.ConnectivityTypeId == id);

            if (connectivityType == null)
                return null;

            return MapToResponse(connectivityType);
        }

        // CREATE
        public async Task<ConnectivityTypeResponse>
            CreateConnectivityType(
                ConnectivityTypeRequest request)
        {
            var connectivityType = new ConnectivityType
            {
                Name = request.Name
            };

            _context.ConnectivityTypes.Add(connectivityType);

            await _context.SaveChangesAsync();

            return MapToResponse(connectivityType);
        }

        // UPDATE
        public async Task<ConnectivityTypeResponse?>
            UpdateConnectivityType(
                int id,
                ConnectivityTypeRequest request)
        {
            var connectivityType =
                await _context.ConnectivityTypes
                    .FirstOrDefaultAsync(
                        c => c.ConnectivityTypeId == id);

            if (connectivityType == null)
                return null;

            connectivityType.Name = request.Name;

            await _context.SaveChangesAsync();

            return MapToResponse(connectivityType);
        }

        // DELETE
        public async Task<bool> DeleteConnectivityType(int id)
        {
            var connectivityType =
                await _context.ConnectivityTypes
                    .FirstOrDefaultAsync(
                        c => c.ConnectivityTypeId == id);

            if (connectivityType == null)
                return false;

            _context.ConnectivityTypes.Remove(connectivityType);

            await _context.SaveChangesAsync();

            return true;
        }

        // MAP ENTITY → RESPONSE
        private ConnectivityTypeResponse MapToResponse(
            ConnectivityType connectivityType)
        {
            return new ConnectivityTypeResponse
            {
                ConnectivityId =
                    connectivityType.ConnectivityTypeId,

                ConnectivityName = connectivityType.Name
            };
        }
    }
}