using System;
using System.ComponentModel.DataAnnotations;

namespace HomeOwner.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        [Required(ErrorMessage = "Event type is required.")]
        public string EventType { get; set; } // Optional: Meeting, Holiday, etc.
        public string BackgroundColor { get; set; } // Hex color or color name

    }
}
