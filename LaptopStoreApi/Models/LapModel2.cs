namespace LaptopStoreApi.Models
{
    public class LapModel2
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } // so luong ton kho
        public decimal? BigPrice { get; set; }
        public string? Color { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }

    }
}
