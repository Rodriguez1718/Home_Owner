using System;
using System.ComponentModel.DataAnnotations;

namespace HomeOwner.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int PostId { get; set; }
        public ForumPost Post { get; set; }

        [Required]
        public string UserId { get; set; }
        public Users User { get; set; }
    }
}
