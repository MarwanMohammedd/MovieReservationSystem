using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.ViewModels;

namespace MovieReservationSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> LogIn(string? ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            LogInViewModel logInViewModel = new()
            {
                EmailAddress = string.Empty,
                Password = string.Empty,
                RememberMe = false,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
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
                    (user.UserName!, logInViewModel.Password, isPersistent: logInViewModel.RememberMe, lockoutOnFailure: false);

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
                        logInViewModel.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                        ModelState.AddModelError(string.Empty, "Sign In Operation Is Failed");
                    }
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "This Account Is Not Registred Yet");
                }
            }
            logInViewModel.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(logInViewModel);
        }


        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(
            action: "ExternalLoginCallback",
            controller: "Account",
            values: new { ReturnUrl = returnUrl }
            );
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl, string? remoteError)
        {
            var users = _userManager.Users.ToList();
            if (users.Count() == 0)
            {
                returnUrl = returnUrl ?? Url.Content("~/");
                if (remoteError != null)
                {
                    return Content($"<script>alert('Error from external provider: {remoteError}'); window.close();</script>", "text/html");
                }

                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return Content($"<script>alert('Error loading external login information.'); window.close();</script>", "text/html");
                }

                var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
                );

                if (signInResult.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!
                        };

                        await _userManager.CreateAsync(user);
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }

                    await _userManager.AddLoginAsync(user, info);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

            }
            else
            {
                returnUrl = returnUrl ?? Url.Content("~/");
                if (remoteError != null)
                {
                    return Content($"<script>alert('Error from external provider: {remoteError}'); window.close();</script>", "text/html");
                }

                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return Content($"<script>alert('Error loading external login information.'); window.close();</script>", "text/html");
                }

                var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
                );

                if (signInResult.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!
                        };

                        await _userManager.CreateAsync(user);
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    await _userManager.AddLoginAsync(user, info);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

            }
            return Content($"<script>alert('Email claim not received. Please contact support.'); window.close();</script>", "text/html");
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
                var users = _userManager.Users.ToList();
                if (users.Count() == 0)
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
                        await _userManager.AddToRoleAsync(user, "Admin");
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
                else
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
                        await _userManager.AddToRoleAsync(user, "User");
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
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(LogIn));
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