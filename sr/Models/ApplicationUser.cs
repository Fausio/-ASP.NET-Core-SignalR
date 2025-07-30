using Microsoft.AspNetCore.Identity;

namespace sr.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public List<NotificationApplicationUser> NotificationApplicationUsers { get; set; }
    }
}
