namespace Backend.DTOs.Response
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        // Specs
        public string ChipName { get; set; } = string.Empty;
        public string CpuCores { get; set; } = string.Empty;
        public string GpuCores { get; set; } = string.Empty;
        public int RamGb { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string DisplayResolution { get; set; } = string.Empty;
        public int MainCameraMp { get; set; }
        public int FrontCameraMp { get; set; }
        public string OsVersion { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        // Variants
        public List<ProductVariantResponse> Variants { get; set; } = new();
    }
}
