using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebAppIdentityEntity.Models;
using WebIdentityEntity.Models;
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
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPassword { Token = token, Email = email});
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return View("Success");
                }
            }

            return View("Error");
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
                        NomeCompleto = registerViewModel.UserName,
                        Email = registerViewModel.Email,    
                    };

                    var result = await _userManager.CreateAsync(myUser, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(myUser);
                        var confirmationEmail = Url.Action("ConfirmEmailAddress", "User",
                                                new { token = token, email = myUser.Email }, Request.Scheme);

                        System.IO.File.WriteAllText("ConfirmEmail.txt", confirmationEmail);
                    }
                    else
                    {
                        foreach (var erro in result.Errors)
                        {
                            ModelState.AddModelError("", erro.Description);
                        }

                        return View();
                    }

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
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "E-mail não está válido!");
                        return View();
                    }

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

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetUrl = Url.Action("ResetPassword", "User",
                        new { token = token, email = model.Email }, Request.Scheme);

                    System.IO.File.WriteAllText("resetLink.txt", resetUrl);

                    return View("Success");
                }
                else
                {

                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var erro in result.Errors)
                        {
                            ModelState.AddModelError("", erro.Description);
                        }

                        return View();
                    }
                    return View("Success");
                }

                ModelState.AddModelError("", "Invalid Request");
            }
            return View();
        }

    }
}
