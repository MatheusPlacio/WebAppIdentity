using System.ComponentModel.DataAnnotations;

namespace JWT.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string NomeCompleto { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
