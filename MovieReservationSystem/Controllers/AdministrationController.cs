using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.ViewModels;

namespace MovieReservationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            RolesViewModel rolesViewModel = new()
            {
                RoleName = string.Empty,
                Roles = await roleManager.Roles.ToListAsync(),
            };
            return View(rolesViewModel);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            RolesViewModel rolesViewModel = new()
            {
                RoleName = string.Empty,
            };
            return View(rolesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RolesViewModel rolesViewModel)
        {
            bool exists = await roleManager.RoleExistsAsync(rolesViewModel.RoleName);
            if (exists)
            {
                ModelState.AddModelError(string.Empty, "This Role Is AleardyExits");
            }
            else
            {
                ApplicationRole identityRole = new()
                {
                    Name = rolesViewModel.RoleName,
                };
                var result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
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
            return View(rolesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            ApplicationRole? exists = await roleManager.FindByNameAsync(roleName);
            if (exists is not null)
            {
                IdentityResult result = await roleManager.DeleteAsync(exists);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This Role Is Already Not Exist");
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> AddUsersInRoles()
        {
            UserRoleViewModel userRoleViewModel = new()
            {
                RoleName = string.Empty,
                UserEmail = string.Empty,
                AvailableRoles = await roleManager.Roles.Select(element => element.Name).ToListAsync(),
            };
            return View(userRoleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUsersInRoles(UserRoleViewModel userRoleViewModel)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(userRoleViewModel.UserEmail);
            ApplicationRole? role = await roleManager.FindByNameAsync(userRoleViewModel.RoleName);
            if (user is not null && role is not null)
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, userRoleViewModel.RoleName);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUsersInRoles()
        {
            UserRoleViewModel userRoleViewModel = new()
            {
                AvailableRoles = await roleManager.Roles.Select(element => element.Name).ToListAsync()
            };
            return View(userRoleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUsersInRolesAsync(UserRoleViewModel userRoleViewModel)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(userRoleViewModel.UserEmail);
            ApplicationRole? role = await roleManager.FindByNameAsync(userRoleViewModel.RoleName);
            if (user is not null && role is not null)
            {
                IdentityResult result = await userManager.RemoveFromRoleAsync(user, userRoleViewModel.RoleName);
                if (result.Succeeded)
                {
                    return RedirectToAction("", "");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReadUsersRole()
        {
            var users = await userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRolesViewModel.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email!,
                    AvailableRoles = roles.ToList()
                });
            }

            return View(userRolesViewModel);
        }
    }
}