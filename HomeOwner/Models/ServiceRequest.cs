using System;
using System.ComponentModel.DataAnnotations;

namespace HomeOwner.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Declined

        public string? AssignedTo { get; set; }

        // New fields for approval system
        public string? ApprovedBy { get; set; } // Admin or Staff who approved/declined
        public DateTime? ApprovalDate { get; set; } // Date of approval/decline
        public string? CreatedBy { get; set; } // Stores the UserId of the Homeowner

    }
}
