using HomeOwner.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeOwner.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set default value for the Role property in the Users table
            builder.Entity<Users>()
                   .Property(u => u.Role)
                   .HasDefaultValue("HomeOwner");
        }

        public DbSet<Billing> Billings { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}

