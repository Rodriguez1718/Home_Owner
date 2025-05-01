using System;
using System.ComponentModel.DataAnnotations;

namespace HomeOwner.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? AdminId { get; set; }

        public virtual Users? Admin { get; set; }  // Link to Users table
    }
}
