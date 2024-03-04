using Microsoft.AspNetCore.Identity;

namespace WebIdentityEntity.Models
{
    public class NaoContemValidadorSenha<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
           var userName = await manager.GetUserNameAsync(user);

            if (userName == password)
                return IdentityResult.Failed(
                    new IdentityError { Description = "A senha não poder igual ao Password"}
                );

            if (password.Contains("password"))
                return IdentityResult.Failed(
                       new IdentityError { Description = "A senha não pode ser password" }
                   );

            return IdentityResult.Success;
        }

    }
}
