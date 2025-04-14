// Models/ServiceRequest.cs
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HomeOwner.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }
    }
}
public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Request Type")]
        public string RequestType { get; set; } // e.g., Maintenance, Security

        [Required]
        public string Description { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";
    }
}
