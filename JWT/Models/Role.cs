using Microsoft.AspNetCore.Identity;

namespace JWT.Models
{
    public class Role : IdentityRole<int>
    {

        //EF
        public ICollection<UserRole> UserRole { get; set; }
    }
}
