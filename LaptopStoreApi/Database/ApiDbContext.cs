﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LaptopStoreApi.Database
{
    public class ApiDbContext : IdentityDbContext<User>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });

            modelBuilder.Entity<Laptop>().HasKey(l => l.LaptopId);

            modelBuilder.Entity<Cart>().HasKey(c=>c.Id);

            modelBuilder.Entity<Cart>()
                       .HasOne(c => c.User)
                       .WithMany()
                       .HasForeignKey(c => c.UserId);

             modelBuilder.Entity<Cart>()
                       .HasOne(c => c.Laptop)
                       .WithMany()
                       .HasForeignKey(c => c.LaptopId);          
                       
            modelBuilder.Entity<Image>()
                .HasOne(i => i.LaptopStatus)
                .WithMany(ls => ls.Images)
                .HasForeignKey(i => i.LaptopStatusId);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Laptop)
                .WithMany()
                .HasForeignKey(o => o.LaptopId);

        }
        public DbSet<Laptop> Laptops => Set<Laptop>();
        public DbSet<LaptopStatus> LaptopStatuses => Set<LaptopStatus>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<Order> Orders => Set<Order>();

    }
}
