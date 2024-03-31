using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LaptopStoreApi.Database
{
    public class ApiDbContext : IdentityDbContext<User>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<IdentityUserLogin<string>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });


            modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Category>().HasMany(c => c.LaptopCategories).WithOne(lc => lc.Category);

            modelBuilder.Entity<Laptop>().HasKey(l => l.LaptopId);
            modelBuilder.Entity<Laptop>().HasMany(l => l.LaptopCategories).WithOne(lc => lc.Laptop);
            modelBuilder.Entity<Laptop>().HasMany(l => l.LikeProducts).WithOne(oi => oi.Laptop);
            //LaptopCategory
            modelBuilder.Entity<LaptopCategory>().HasKey(lc => new { lc.LaptopId, lc.CategoryId });
            modelBuilder.Entity<LaptopCategory>().HasOne(lc => lc.Laptop).WithMany(l => l.LaptopCategories).HasForeignKey(lc => lc.LaptopId);
            modelBuilder.Entity<LaptopCategory>().HasOne(lc => lc.Category).WithMany(c => c.LaptopCategories).HasForeignKey(lc => lc.CategoryId);

            //LikeProducts
            modelBuilder.Entity<LikeProduct>().HasKey(lp => new { lp.LaptopId, lp.UserId });
            modelBuilder.Entity<LikeProduct>().HasOne(lp => lp.Laptop).WithMany(l => l.LikeProducts).HasForeignKey(lp => lp.LaptopId);
            modelBuilder.Entity<LikeProduct>().HasOne(lp => lp.User).WithMany(l => l.LikeProducts).HasForeignKey(lp => lp.UserId);

            modelBuilder.Entity<LikeProduct>().HasOne(lp => lp.Laptop).WithMany(l => l.LikeProducts).HasForeignKey(lp => lp.LaptopId);

            modelBuilder.Entity<Cart>().HasKey(c => c.Id);

            modelBuilder.Entity<Cart>().HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);

            modelBuilder
                .Entity<Cart>()
                .HasOne(c => c.Laptop)
                .WithMany()
                .HasForeignKey(c => c.LaptopId);

            modelBuilder
                .Entity<Image>()
                .HasOne(i => i.LaptopStatus)
                .WithMany(ls => ls.Images)
                .HasForeignKey(i => i.LaptopStatusId);
            modelBuilder
                .Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);
            modelBuilder
                .Entity<Order>()
                .HasOne(o => o.Laptop)
                .WithMany()
                .HasForeignKey(o => o.LaptopId);
            modelBuilder
                .Entity<Evaluate>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);





            modelBuilder.Entity<Evaluate>().HasOne(e => e.Laptop).WithMany().HasForeignKey(e => e.LaptopId);
            modelBuilder.Entity<Cart>().Property(c => c.Price).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Laptop>().Property(l => l.Price).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Laptop>().Property(l => l.BigPrice).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>().Property(o => o.Price).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>().Property(o => o.Total).HasColumnType("decimal(18, 2)");
            // .OnDelete(DeleteBehavior.Cascade); // Nếu bạn muốn xóa tất cả LaptopStatus liên quan khi Laptop bị xóa


            // Thiết lập Id làm khóa chính
            modelBuilder.Entity<OrderOffline>().HasKey(l => l.Id);

            // Thiết lập cột Id để tự động tăng dần
            modelBuilder.Entity<OrderOffline>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserGroupRole>().HasKey(lc => new { lc.UserId, lc.GroupRoleId });
            modelBuilder.Entity<UserGroupRole>().HasOne(lc => lc.User).WithMany(l => l.UserGroupRole).HasForeignKey(lc => lc.UserId);
            modelBuilder.Entity<UserGroupRole>().HasOne(lc => lc.GroupRole).WithMany(c => c.UserGroups).HasForeignKey(lc => lc.GroupRoleId);

        }
        public DbSet<Laptop> Laptops => Set<Laptop>();
        public DbSet<LaptopStatus> LaptopStatuses => Set<LaptopStatus>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Evaluate> Evaluates => Set<Evaluate>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<LaptopCategory> LaptopCategories { get; set; }
        public DbSet<LikeProduct> LikeProducts { get; set; }
        public DbSet<Promotion> Promotions { get; set; } // Khuyến mãi
        public DbSet<OrderOffline> OrderOfflines { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }
        public DbSet<UserGroupRole> UserGroups { get; set; }
    }
}
