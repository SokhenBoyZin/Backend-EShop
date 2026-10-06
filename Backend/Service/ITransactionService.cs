using Backend.DTOs.Transaction;

namespace Backend.Services
{
    public interface ITransactionService
    {
        Task<TransactionResponse?> CreateTransactionAsync(TransactionRequest request, int userId);

        Task<List<TransactionResponse>> GetMyTransactionsAsync(int userId);

        Task<TransactionResponse?> GetByIdAsync(int transactionId, int userId);

        Task<List<TransactionResponse>> GetAllTransactionsAsync();

        Task<TransactionResponse?> AdminGetByIdAsync(int transactionId);
        Task<bool> AdminUpdatePaymentAsync(int transactionId, TransactionUpdateRequest request);

        Task<VerifyPaymentResponse?> VerifyPaymentAsync(int transactionId, int userId, string? transactionRef);

        Task<bool> CancelTransactionAsync(int transactionId, int userId);

    }
}