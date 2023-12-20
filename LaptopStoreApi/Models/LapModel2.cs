namespace LaptopStoreApi.Models
{
    public class LapModel2
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } // so luong ton kho
        public IFormFile? Image { get; set; }

    }
}
