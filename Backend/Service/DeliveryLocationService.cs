using Backend.Db;
using Backend.DTOs.DeliveryLocation;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Services
{
    public class DeliveryLocationService : IDeliveryLocationService
    {
        private readonly ApplicationDbContext _context;

        public DeliveryLocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET MY LOCATIONS
        public async Task<List<DeliveryLocationResponse>> GetMyLocationsAsync(
            int userId)
        {
            return await _context.DeliveryLocations
                .Where(x => x.UserId == userId)
                .Select(x => new DeliveryLocationResponse
                {
                    DeliveryLocationId = x.DeliveryLocationId,
                    ContactName = x.ContactName,
                    PhoneNumber = x.PhoneNumber,
                    City = x.City,
                    District = x.District,
                    Address = x.Address
                })
                .ToListAsync();
        }


        // GET LOCATION BY ID
        public async Task<DeliveryLocationResponse?> GetByIdAsync(int id, int userId)
        {
            return await _context.DeliveryLocations
                .Where(x =>
                    x.DeliveryLocationId == id &&
                    x.UserId == userId)
                .Select(x => new DeliveryLocationResponse
                {
                    DeliveryLocationId = x.DeliveryLocationId,
                    ContactName = x.ContactName,
                    PhoneNumber = x.PhoneNumber,
                    City = x.City,
                    District = x.District,
                    Address = x.Address
                })
                .FirstOrDefaultAsync();
        }


        // CREATE
        public async Task<DeliveryLocationResponse?> CreateAsync(DeliveryLocationRequest request, int userId)
        {
            if (request == null) return null;

            if (string.IsNullOrWhiteSpace(request.ContactName) ||
                string.IsNullOrWhiteSpace(request.PhoneNumber) ||
                string.IsNullOrWhiteSpace(request.City) ||
                string.IsNullOrWhiteSpace(request.District) ||
                string.IsNullOrWhiteSpace(request.Address))
            {
                return null;
            }

            var location = new DeliveryLocation
            {
                UserId = userId,
                ContactName = request.ContactName,
                PhoneNumber = request.PhoneNumber,
                City = request.City,
                District = request.District,
                Address = request.Address
            };

            _context.DeliveryLocations.Add(location);

            await _context.SaveChangesAsync();

            return new DeliveryLocationResponse
            {
                DeliveryLocationId = location.DeliveryLocationId,
                ContactName = location.ContactName,
                PhoneNumber = location.PhoneNumber,
                City = location.City,
                District = location.District,
                Address = location.Address
            };
        }


        // UPDATE
        public async Task<DeliveryLocationResponse?> UpdateAsync(
            int id,
            DeliveryLocationRequest request,
            int userId)
        {
            if (request == null)
                return null;

            var location = await _context.DeliveryLocations
                .FirstOrDefaultAsync(x =>
                    x.DeliveryLocationId == id &&
                    x.UserId == userId);

            if (location == null)
                return null;

            location.ContactName = request.ContactName;
            location.PhoneNumber = request.PhoneNumber;
            location.City = request.City;
            location.District = request.District;
            location.Address = request.Address;

            await _context.SaveChangesAsync();

            return new DeliveryLocationResponse
            {
                DeliveryLocationId = location.DeliveryLocationId,
                ContactName = location.ContactName,
                PhoneNumber = location.PhoneNumber,
                City = location.City,
                District = location.District,
                Address = location.Address
            };
        }


        // DELETE
        public async Task<bool> DeleteAsync(
            int id,
            int userId)
        {
            var location = await _context.DeliveryLocations
                .FirstOrDefaultAsync(x =>
                    x.DeliveryLocationId == id &&
                    x.UserId == userId);

            if (location == null) return false;

            _context.DeliveryLocations.Remove(location);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}