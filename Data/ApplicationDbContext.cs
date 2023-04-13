using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LicentaFinal.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LicentaFinal.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace LicentaFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
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
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }


        public DbSet<LicentaFinal.Models.Order> Order { get; set; }
        public DbSet<LicentaFinal.Models.OrderItem> OrderItem { get; set; }
        public DbSet<LicentaFinal.Models.OrderHistory>? OrderHistory { get; set; }
        public DbSet<LicentaFinal.Models.OrderInvoiceHistory>? OrderInvoiceHistory { get; set; }
    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(255);
            builder.Property(u => u.LastName).HasMaxLength(255);
        }
    }
}