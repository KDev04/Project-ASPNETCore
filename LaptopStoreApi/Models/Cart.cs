namespace LaptopStoreApi.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();
        public virtual void AddItem(Laptop laptop, int quantity)
        {
            CartLine? line = Lines
                .Where(p => p.Laptop.Id == laptop.Id)
                .FirstOrDefault();
            if (line == null)
            {
                Lines.Add(new CartLine { Laptop = laptop, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Laptop L) => Lines.RemoveAll(l => l.Laptop.Id == L.Id);
        public decimal ComputeTotalValue() => Lines.Sum(e => e.Laptop.Price * e.Quantity);
        public virtual void Clear() => Lines.Clear();


    }
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Laptop Laptop { get; set; } = new();
        public int Quantity { get; set; }
    }
}
