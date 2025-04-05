using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

    }
}