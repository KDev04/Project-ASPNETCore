using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LaptopStoreApi.Database
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public ICollection<CartItem>? Items { get; set; }
/*        public virtual void Add(Laptop laptop, int quantity)
        {
            CartItem? item = Items.Where(i => i.Laptop.IdLaptop == laptop.IdLaptop).FirstOrDefault();
            if (item == null)
            {
                Items.Add(new CartItem { Laptop = laptop, Quantity = quantity });
            }
            else { item.Quantity += quantity; }
        }
        public virtual void RemoveLine(Laptop L) => Items.RemoveAll(l => l.Laptop.IdLaptop == L.IdLaptop);
        public decimal ComputeTotalValue() => Items.Sum(e => e.Laptop.Price * e.Quantity);
        public virtual void Clear() => Items.Clear();*/
    }
}
