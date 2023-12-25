namespace LaptopStore.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public DateTime BirthDay { get; set; } = DateTime.MinValue;
        public string Address { get; set; } = null!;
        public int Age { get; set; }


    }
}
