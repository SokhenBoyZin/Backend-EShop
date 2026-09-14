using Backend.Models;

namespace Backend.DTOs.Transaction
{
    public class TransactionResponse
    {
        public int TransactionId { get; set; }
        public int OrderId { get; set; }
        public int PaymentMethodId { get; set; }
        public string BankName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string? TransactionRef { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}