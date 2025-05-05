using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeOwner.Models
{
    public class CommunityForum
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ForumPost> Posts { get; set; }
    }

    public class ForumPost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ForumId { get; set; }
        public CommunityForum Forum { get; set; }
    }
}
