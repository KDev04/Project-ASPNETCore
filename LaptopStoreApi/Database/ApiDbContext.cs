using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LaptopStoreApi.Database
{
    public class ApiDbContext: IdentityDbContext<User>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
        }
        public DbSet<Laptop>Laptops => Set<Laptop>();
        public DbSet<LaptopStatus> LaptopStatuses => Set<LaptopStatus>();
    }
}
