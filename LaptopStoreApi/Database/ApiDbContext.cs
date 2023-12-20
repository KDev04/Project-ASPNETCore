﻿using Microsoft.AspNetCore.Identity;
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
            modelBuilder.Entity<Laptop>().HasKey(l => l.LaptopId);
            modelBuilder.Entity<Cart>()
                       .HasOne(c => c.User)
                       .WithMany()
                       .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Laptop)
                .WithMany()
                .HasForeignKey(ci => ci.LaptopId);
            modelBuilder.Entity<Image>()
                .HasOne(i => i.LaptopStatus)
                .WithMany(ls => ls.Images)
                .HasForeignKey(i => i.LaptopStatusId);

        }
        public DbSet<Laptop>Laptops => Set<Laptop>();
        public DbSet<LaptopStatus> LaptopStatuses => Set<LaptopStatus>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

    }
}
