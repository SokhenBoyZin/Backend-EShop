namespace Backend.DTOs.Analytics
{
    public class RevenueRequest
    {
        public string Period { get; set; } = "day";

        public int Year { get; set; }

        public int? Month { get; set; }
    }
}