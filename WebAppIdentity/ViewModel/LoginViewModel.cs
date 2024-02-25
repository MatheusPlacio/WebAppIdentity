using System.ComponentModel.DataAnnotations;

namespace WebAppIdentity.ViewModel
{
    public class LoginViewModel
    {
        public string UserName { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
