using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeOwner.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reservation date is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Reservation), nameof(ValidateReservationDate))]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Please enter a purpose.")]
        [StringLength(255, ErrorMessage = "Purpose can't be longer than 255 characters.")]
        public string Purpose { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Facility selection is required.")]
        public int FacilityId { get; set; }

        // Navigation Properties
        public Facility? Facility { get; set; }
        public Users? User { get; set; }

        // Custom validation method
        public static ValidationResult? ValidateReservationDate(DateTime date, ValidationContext context)
        {
            if (date.Date < DateTime.Today)
            {
                return new ValidationResult("Reservation date cannot be in the past.");
            }
            return ValidationResult.Success;
        }
    }
}
