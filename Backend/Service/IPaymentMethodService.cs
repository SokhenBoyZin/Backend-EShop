using Backend.DTOs.PaymentMethod;

namespace Backend.Service
{
    public interface IPaymentMethodService
    {
        Task<List<PaymentMethodResponse>> GetAllAsync();
        Task<PaymentMethodResponse?> GetByIdAsync(int id);
        Task<PaymentMethodResponse?> CreateAsync(PaymentMethodRequest request);
        Task<PaymentMethodResponse?> UpdateAsync(int id, PaymentMethodRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
