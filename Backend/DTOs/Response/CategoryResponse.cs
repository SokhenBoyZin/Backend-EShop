using Backend.Models;

namespace Backend.DTOs.Response
{
    public class CategoryResponse
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
