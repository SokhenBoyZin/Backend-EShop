using Backend.Models;

namespace Backend.DTOs.Transaction
{
    public class TransactionUpdateRequest
    {
        public PaymentStatus PaymentStatus { get; set; }

        public string? TransactionRef { get; set; }
    }
}