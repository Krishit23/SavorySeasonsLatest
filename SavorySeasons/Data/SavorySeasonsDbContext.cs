using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SavorySeasons.Entities;
using SavorySeasons.Models;

namespace SavorySeasons.Data
{
    public class SavorySeasonsDbContext : IdentityDbContext<ApplicationUser>
    {
        public SavorySeasonsDbContext(DbContextOptions<SavorySeasonsDbContext> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>(_ =>
            {
                _.ToTable(Order.OrderTable);
                _.HasKey(_ => _.Id);
                _.HasOne(_ => _.ApplicationUser).WithMany().HasForeignKey(_ => _.userId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
