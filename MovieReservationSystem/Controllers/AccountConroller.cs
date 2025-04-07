using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.ViewModels;
using System.Security.Claims;

namespace MovieReservationSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult LogIn(string? ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            LogInViewModel logInViewModel = new()
            {
                EmailAddress = string.Empty,
                Password = string.Empty,
                RememberMe = false,
            };
            return View(logInViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel logInViewModel, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(logInViewModel.EmailAddress);
                if (user is not null)
                {
                    var result = await _signInManager.PasswordSignInAsync
                     (user.UserName, logInViewModel.Password, isPersistent: logInViewModel.RememberMe, lockoutOnFailure: false);


                    if (result.Succeeded)
                    {
                        var userId = user.Id;

                        if (!string.IsNullOrEmpty(ReturnUrl))
                        {
                            // check is local url for prevent open redirect attack
                            return LocalRedirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Sign In Operation Is Failed");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This Account Is Not Registred Yet");
                }
            }
            return View(logInViewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new()
            {
                LastName = string.Empty,
                FirstName = string.Empty,
                ConfirmPassword = string.Empty,
                EmailAddress = string.Empty,
                Password = string.Empty,
                UserName = string.Empty,
            };
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    PhoneNumber = registerViewModel.PhoneNumber,
                    Email = registerViewModel.EmailAddress,
                    UserName = registerViewModel.UserName,
                };
                IdentityResult result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
            RedirectToAction(nameof(LogIn));
        }

        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string? EmailAddress)
        {
            if (!string.IsNullOrEmpty(EmailAddress))
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(EmailAddress);
                if (user is not null)
                {
                    return Json($"{EmailAddress} Is Already Taken.");
                }
            }
            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> IsUserNameAvailable(string? UserName)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                ApplicationUser? user = await _userManager.FindByNameAsync(UserName);
                if (user is not null)
                {
                    return Json($"{UserName} Is Already Taken.");
                }
            }
            return Json(true);
        }

        [HttpGet]
        
        public IActionResult AccessDenied()
        {
            return View();
        }

    }

}