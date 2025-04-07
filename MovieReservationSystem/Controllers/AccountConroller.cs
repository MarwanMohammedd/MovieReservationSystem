using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.Controllers
{
    public class AccountConroller : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountConroller(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

    }
}