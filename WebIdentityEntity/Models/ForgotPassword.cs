using System.ComponentModel.DataAnnotations;

namespace WebIdentityEntity.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
