using Microsoft.EntityFrameworkCore;
namespace LaptopStoreApi.Data
{
    public class ApplicationLaptopDbContext : DbContext
    {
        public ApplicationLaptopDbContext(DbContextOptions<ApplicationLaptopDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHang>(e =>
            {
                e.ToTable("DonHangs");
                e.HasKey(dh => dh.MaDh);
                e.Property(dh => dh.NgayDat).HasDefaultValueSql("getutcdate()");
            });
            modelBuilder.Entity<DonHangChiTiet>(e =>
            {
                e.ToTable("ChiTietDonHangs");
                e.HasKey(e=> new { e.MaDh, e.MaLaptop });
                e.HasOne(e => e.DonHang)
                    .WithMany(e => e.DonHangChiTiets)
                    .HasForeignKey(e => e.MaDh)
                    .HasConstraintName("FK_DonHangCT_DonHang");
                e.HasOne(e => e.Laptop)
                    .WithMany(e => e.DonHangChiTiets)
                    .HasForeignKey(e => e.MaLaptop)
                    .HasConstraintName("FK_DonHangCT_Laptop");

            });
        }
        public DbSet<Laptop> Laptops => Set<Laptop>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<DonHangChiTiet> DonHangChiTiets => Set<DonHangChiTiet>();
        public DbSet<DonHang> DonHangs => Set<DonHang>();
        public DbSet<Homepage> Homepages => Set<Homepage>();
    }
}
