using Microsoft.AspNetCore.Identity;

namespace HomeOwner.Models
{
    public class Users : IdentityUser
    {
        public string? FullName { get; set; }



    }
}
