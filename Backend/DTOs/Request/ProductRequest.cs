namespace Backend.DTOs.Request
{
    public class ProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string Image { get; set; } = string.Empty;
        public string ChipName { get; set; } = string.Empty;
        public string CpuCores { get; set; } = string.Empty;
        public string GpuCores { get; set; } = string.Empty; 
        public int RamGb { get; set; } 
        public string DisplayName { get; set; } = string.Empty; 
        public string DisplayResolution { get; set; } = string.Empty; 
        public int MainCameraMp { get; set; } 
        public int FrontCameraMp { get; set; } 
        public string OsVersion { get; set; } = string.Empty;
    }
}
