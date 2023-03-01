using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LicentaFinal.Models;

namespace LicentaFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Order>().Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Entity<OrderItem>().Property(t => t.Id).ValueGeneratedOnAdd();
        }

        public DbSet<LicentaFinal.Models.Stocuri> Stocuri { get; set; }
        public DbSet<LicentaFinal.Models.Furnizori> Furnizori { get; set; }
        public DbSet<LicentaFinal.Models.Order> Order { get; set; }
        public DbSet<LicentaFinal.Models.OrderItem> OrderItem { get; set; }
    }
}