using System.ComponentModel.DataAnnotations;

namespace WebIdentityEntity.Models
{
    public class ResetPassword
    {
        public string Token { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
