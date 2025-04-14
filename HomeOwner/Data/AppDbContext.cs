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

        public DbSet<Announcement> Announcements { get; set; } // Add this line

        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Announcement>()
                .HasOne(a => a.Admin)
                .WithMany(u => u.Announcements) // if you add the collection in Users.cs
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
