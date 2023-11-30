using Microsoft.EntityFrameworkCore;
namespace LaptopStoreApi.Models
{
    public class ApplicationLaptopDbContext : DbContext
    {
        public ApplicationLaptopDbContext(DbContextOptions<ApplicationLaptopDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Laptop> Laptops => Set<Laptop>();
    }
}
