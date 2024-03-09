namespace LaptopStoreApi.Database
{
    public class LaptopCategory
    {
        public int LaptopId { get; set; }

        public Laptop Laptop { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
