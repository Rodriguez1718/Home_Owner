using HomeOwner.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeOwner.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty; // Foreign key
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Purpose { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public int FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;

        public Users? User { get; set; } // Navigation property
    }
}
