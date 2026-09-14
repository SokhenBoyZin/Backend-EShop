namespace Backend.DTOs.Transaction
{
    public class TransactionRequest
    {
        public int OrderId { get; set; }
        public int PaymentMethodId { get; set; }
    }
}