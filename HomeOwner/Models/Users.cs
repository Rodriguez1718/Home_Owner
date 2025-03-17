using Microsoft.AspNetCore.Identity;

namespace HomeOwner.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }

        public string Role { get; set; } = "HomeOwner";

        public string Payment { get; set; }
        public string Billing { get; set; }
    }
}
