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

        public DbSet<Announcement> Announcements { get; set; } // Add this lineeeee
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Event> Events { get; set; }



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
