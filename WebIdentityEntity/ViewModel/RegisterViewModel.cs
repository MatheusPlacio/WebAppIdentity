using System.ComponentModel.DataAnnotations;

namespace WebIdentityEntity.ViewModel
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
