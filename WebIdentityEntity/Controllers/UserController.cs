using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebAppIdentityEntity.Models;
using WebIdentityEntity.ViewModel;

namespace WebIdentityEntity.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<MyUser> _userManager;

        public UserController(ILogger<UserController> logger,
                              UserManager<MyUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(registerViewModel.UserName);

                if (user.Result == null)
                {
                    MyUser myUser = new MyUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = registerViewModel.UserName,
                        NomeCompleto = registerViewModel.UserName
                    };

                    var result = await _userManager.CreateAsync(myUser, registerViewModel.Password);

                }

                return View("Success");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);

                if (user != null && checkPassword)
                {
                    var identity = new ClaimsIdentity("cookies");
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                    await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));

                    return RedirectToAction("About");
                }

                ModelState.AddModelError("", "Usuário ou senha Inválida");
            }

            return View();
        }

    }
}
