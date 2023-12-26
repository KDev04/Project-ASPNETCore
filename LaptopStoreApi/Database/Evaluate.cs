using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Database
{
    public class Evaluate
    {
        [Key]
        public int Id { get; set; }
        [Range(0, 5)]
        public int Rate { get; set; } 
        public string Cmt { get; set; } = null!;
        public int LaptopId { get; set; }
        public Laptop Laptop { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

    }
}
