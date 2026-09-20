namespace Backend.DTOs.Analytics
{
    public class RevenueResponse
    {
        public string Period { get; set; } = string.Empty;

        public int Year { get; set; }

        public int? Month { get; set; }

        public decimal TotalRevenue { get; set; }

        public List<RevenueDataResponse> Data { get; set; } = new();
    }

    public class RevenueDataResponse
    {
        public string Label { get; set; } = string.Empty;

        public decimal Revenue { get; set; }
    }
}