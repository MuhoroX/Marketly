using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
