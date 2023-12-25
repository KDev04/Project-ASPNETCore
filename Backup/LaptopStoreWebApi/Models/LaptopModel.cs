namespace LaptopStoreWebApi.Models
{
    public class LaptopModel
    {
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; } // so luong ton kho
        public IFormFile? Image { get; set; }
    }
}
