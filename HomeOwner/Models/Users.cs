using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HomeOwner.Models
{
    public class Users : IdentityUser
    {
        public string? FullName { get; set; }

        // Navigation property for announcements created by the user  
        public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    }
}
