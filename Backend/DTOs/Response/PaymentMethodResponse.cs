namespace Backend.DTOs.PaymentMethod
{
    public class PaymentMethodResponse
    {
        public int PaymentMethodId { get; set; }
        public string BankName { get; set; } = string.Empty;
    }
}
