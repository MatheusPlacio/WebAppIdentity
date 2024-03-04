using Microsoft.AspNetCore.Identity;

namespace JWT.Models
{
    public class User : IdentityUser<int>
    {
        public string NomeCompleto { get; set; }
        public string Member { get; set; } = "Member";

        //EF
        public ICollection<UserRole> UserRole { get; set; }
    }
}
