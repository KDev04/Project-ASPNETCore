using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace LaptopStoreApi.Data
{
    public class ApplicationLaptopDbContext : IdentityDbContext<ApiUser>
    {
        public ApplicationLaptopDbContext(DbContextOptions<ApplicationLaptopDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<Laptop>()
                .HasKey(l => l.MaLaptop);
            modelBuilder.Entity<DonHang>(e =>
            {
                e.ToTable("DonHangs");
                e.HasKey(dh => dh.MaDh);
                e.Property(dh => dh.NgayDat).HasDefaultValueSql("getutcdate()");
            });
            modelBuilder.Entity<DonHangChiTiet>(e =>
            {
                e.ToTable("ChiTietDonHangs");
                e.HasKey(e=> new { e.MaDh });
                e.HasOne(e => e.DonHang)
                    .WithMany(e => e.DonHangChiTiets)
                    .HasForeignKey(e => e.MaDh)
                    .HasConstraintName("FK_DonHangCT_DonHang");
                e.HasOne(e => e.Laptop)
                    .WithMany(e => e.DonHangChiTiets)
                    .HasForeignKey(e => e.MaLaptop)
                    .HasConstraintName("FK_DonHangCT_Laptop");

            });
            modelBuilder.Entity<Homepage>()
            .HasOne(h => h.Laptop)
            .WithMany() // Mỗi laptop có thể xuất hiện trong nhiều homepage, bạn có thể điều chỉnh nếu cần thiết
            .HasForeignKey(h => h.MaLaptop)
            .HasConstraintName("FK_Homepage_Laptops");
        }
        public DbSet<Laptop> Laptops => Set<Laptop>();
   /*     public DbSet<Category> Categories => Set<Category>();*/
        public DbSet<DonHangChiTiet> DonHangChiTiets => Set<DonHangChiTiet>();
        public DbSet<DonHang> DonHangs => Set<DonHang>();
        public DbSet<Homepage> Homepages => Set<Homepage>();
    }
}
