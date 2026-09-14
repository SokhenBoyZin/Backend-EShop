
namespace Backend.DTOs.DeliveryLocation
{
    public class DeliveryLocationRequest
    {
        public string ContactName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
