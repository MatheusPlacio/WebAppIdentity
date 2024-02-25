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
        private readonly IUserClaimsPrincipalFactory<MyUser> _userClaimsPrincipalFactory;
        private readonly SignInManager<MyUser> _signInManager;

        public UserController(ILogger<UserController> logger,
                              UserManager<MyUser> userManager,
                              IUserClaimsPrincipalFactory<MyUser> userClaimsPrincipalFactory,
                              SignInManager<MyUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _signInManager = signInManager;
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
                    var princiapl = await _userClaimsPrincipalFactory.CreateAsync(user);

                    await HttpContext.SignInAsync("Identity.Application", princiapl);

                    //var signInResult = await _signInManager.PasswordSignInAsync(model.UserName,
                    //    model.Password, false, false);

                    //if (signInResult.Succeeded)
                    //{
                    //    return View("About");

                    //}

                    return View("About");
                }

                    ModelState.AddModelError("", "Usuário ou senha Inválida");
            }

            return View();
        }

    }
}
