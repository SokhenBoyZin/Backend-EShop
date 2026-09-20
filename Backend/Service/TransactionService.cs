using Backend.Db;
using Backend.DTOs.Transaction;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }


        // CREATE TRANSACTION
        public async Task<TransactionResponse?>CreateTransactionAsync(
                TransactionRequest request,
                int userId)
        {
            if (request == null)
            {
                return null;
            }

            if (request.OrderId <= 0 ||
                request.PaymentMethodId <= 0)
            {
                return null;
            }


            // Check order belongs to current user
            var order = await _context.Orders
                .FirstOrDefaultAsync(x =>
                    x.OrderId == request.OrderId &&
                    x.UserId == userId);

            if (order == null)
            {
                return null;
            }


            // Order must still be payable
            if (order.OrderStatus != OrderStatus.PENDING)
            {
                return null;
            }


            // Check payment method exists
            var paymentMethod =
                await _context.PaymentMethods
                    .FirstOrDefaultAsync(x =>
                        x.PaymentMethodId ==
                        request.PaymentMethodId);

            if (paymentMethod == null)
            {
                return null;
            }


            // Create new transaction
            var transaction = new Transaction
            {
                OrderId = order.OrderId,

                PaymentMethodId =
                    paymentMethod.PaymentMethodId,

                Amount = order.TotalAmount,

                PaymentStatus =
                    PaymentStatus.PENDING,

                TransactionRef = null,

                PaidAt = null
            };


            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();


            return MapToResponse(transaction);
        }


        // GET MY TRANSACTIONS
        public async Task<List<TransactionResponse>> GetMyTransactionsAsync(int userId)
        {
            var transactions =
                await _context.Transactions

                    .Include(x =>
                        x.PaymentMethod)

                    .Include(x =>
                        x.Order)

                    .Where(x =>
                        x.Order.UserId == userId)

                    .OrderByDescending(x =>
                        x.TransactionId)

                    .ToListAsync();


            return transactions
                .Select(MapToResponse)
                .ToList();
        }


        // GET TRANSACTION BY ID
        public async Task<TransactionResponse?>
            GetByIdAsync(
                int transactionId,
                int userId)
        {
            var transaction =
                await _context.Transactions

                    .Include(x =>
                        x.PaymentMethod)

                    .Include(x =>
                        x.Order)

                    .FirstOrDefaultAsync(x =>
                        x.TransactionId ==
                            transactionId &&
                        x.Order.UserId ==
                            userId);


            if (transaction == null)
            {
                return null;
            }


            return MapToResponse(transaction);
        }

        // GET ALL USER TRANSACTIONS ONLY ADMIN
        public async Task<List<TransactionResponse>> GetAllTransactionsAsync()
        {
            var transactions = await _context.Transactions
                .Include(x => x.PaymentMethod)
                .Include(x => x.Order)
                .OrderByDescending(x => x.TransactionId)
                .ToListAsync();

            return transactions
                .Select(MapToResponse)
                .ToList();
        }

        // GET BY ID TRANSACTION ONLY ADMIN
        public async Task<TransactionResponse?> AdminGetByIdAsync(int transactionId)
        {
            var transaction = await _context.Transactions
                .Include(x => x.PaymentMethod)
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x =>
                    x.TransactionId == transactionId);

            if (transaction == null)
            {
                return null;
            }

            return MapToResponse(transaction);
        }


        // ADMIN UPDATE PAYMENT
        public async Task<bool> AdminUpdatePaymentAsync(
            int transactionId,
            TransactionUpdateRequest request)
        {
            if (request == null)
            {
                return false;
            }

            var transaction = await _context.Transactions
        .Include(x => x.Order)
            .ThenInclude(x => x.OrderItems)
                .ThenInclude(x => x.ProductVariant)
        .FirstOrDefaultAsync(x =>
            x.TransactionId == transactionId);

            if (transaction == null)
            {
                return false;
            }

            // Transaction can only be updated while PENDING
            if (transaction.PaymentStatus != PaymentStatus.PENDING)
            {
                return false;
            }

            // PAYMENT SUCCESS
            if (request.PaymentStatus == PaymentStatus.PAID)
            {

                // Order must still be PENDING
                if (transaction.Order.OrderStatus != OrderStatus.PENDING)
                {
                    return false;
                }

                // =========================
                // CHECK STOCK FIRST
                // =========================
                foreach (var item in transaction.Order.OrderItems)
                {
                    if (item.ProductVariant == null)
                    {
                        return false;
                    }

                    if (item.Quantity <= 0)
                    {
                        return false;
                    }

                    if (item.ProductVariant.StockQuantity < item.Quantity)
                    {
                        // Not enough stock
                        return false;
                    }
                }

                // =========================
                // REDUCE STOCK
                // =========================
                foreach (var item in transaction.Order.OrderItems)
                {
                    if (item.ProductVariant == null)
                    {
                        return false;
                    }

                    item.ProductVariant.StockQuantity -= item.Quantity;
                }

                // =========================
                // UPDATE TRANSACTION
                // =========================
                transaction.PaymentStatus =
                    PaymentStatus.PAID;

                transaction.TransactionRef =
                    request.TransactionRef;

                transaction.PaidAt =
                    DateTime.UtcNow;


                // =========================
                // UPDATE ORDER
                // =========================
                transaction.Order.OrderStatus =
                    OrderStatus.PROCESSING;

                await _context.SaveChangesAsync();

                return true;
            }

            // =========================
            // PAYMENT FAILED
            // =========================
            else if (request.PaymentStatus == PaymentStatus.FAILED)
            {
                transaction.PaymentStatus =
                    PaymentStatus.FAILED;

                transaction.TransactionRef =
                    request.TransactionRef;

                transaction.PaidAt = null;

                await _context.SaveChangesAsync();

                return true;
            }

            return true;
        }

        // MAP TRANSACTION TO RESPONSE
        private TransactionResponse
            MapToResponse(
                Transaction transaction)
        {
            return new TransactionResponse
            {
                TransactionId =
                    transaction.TransactionId,

                OrderId =
                    transaction.OrderId,

                PaymentMethodId =
                    transaction.PaymentMethodId,

                BankName =
                    transaction.PaymentMethod.BankName,

                Amount =
                    transaction.Amount,

                PaymentStatus =
                    transaction.PaymentStatus,

                TransactionRef =
                    transaction.TransactionRef,

                PaidAt =
                    transaction.PaidAt
            };
        }
    }
}