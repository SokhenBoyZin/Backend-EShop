using Backend.Db;
using Backend.DTOs.PaymentMethod;
using Backend.Models;
using Backend.Service;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly ApplicationDbContext _context;

        public PaymentMethodService(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<List<PaymentMethodResponse>> GetAllAsync()
        {
            return await _context.PaymentMethods
                .Select(x => new PaymentMethodResponse
                {
                    PaymentMethodId = x.PaymentMethodId,
                    BankName = x.BankName
                })
                .ToListAsync();
        }


        // GET BY ID
        public async Task<PaymentMethodResponse?> GetByIdAsync(int id)
        {
            return await _context.PaymentMethods
                .Where(x => x.PaymentMethodId == id)
                .Select(x => new PaymentMethodResponse
                {
                    PaymentMethodId = x.PaymentMethodId,
                    BankName = x.BankName
                })
                .FirstOrDefaultAsync();
        }


        // CREATE
        public async Task<PaymentMethodResponse?> CreateAsync(PaymentMethodRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.BankName))
            {
                return null;
            }

            // Check duplicate
            var exists = await _context.PaymentMethods
                .AnyAsync(x => x.BankName.ToLower() == request.BankName.ToLower());

            if (exists)
                return null;

            var paymentMethod = new PaymentMethod
            {
                BankName = request.BankName.Trim()
            };

            _context.PaymentMethods.Add(paymentMethod);

            await _context.SaveChangesAsync();

            return new PaymentMethodResponse
            {
                PaymentMethodId = paymentMethod.PaymentMethodId,
                BankName = paymentMethod.BankName
            };
        }


        // UPDATE
        public async Task<PaymentMethodResponse?> UpdateAsync(
            int id,
            PaymentMethodRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.BankName))
            {
                return null;
            }

            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(x => x.PaymentMethodId == id);

            if (paymentMethod == null) return null;

            // Check duplicate
            var duplicate = await _context.PaymentMethods
                .AnyAsync(x =>
                    x.PaymentMethodId != id &&
                    x.BankName.ToLower() == request.BankName.ToLower());

            if (duplicate) return null;

            paymentMethod.BankName = request.BankName.Trim();

            await _context.SaveChangesAsync();

            return new PaymentMethodResponse
            {
                PaymentMethodId = paymentMethod.PaymentMethodId,
                BankName = paymentMethod.BankName
            };
        }


        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(x => x.PaymentMethodId == id);

            if (paymentMethod == null)
                return false;

            _context.PaymentMethods.Remove(paymentMethod);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}


