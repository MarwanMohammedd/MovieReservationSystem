using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.ViewModels;

namespace MovieReservationSystem.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole<int>> roleManager, UserManager<ApplicationUser> userManager)
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
                IdentityRole<int> identityRole = new()
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
        public IActionResult DeleteRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(RolesViewModel rolesViewModel)
        {
            IdentityRole<int>? exists = await roleManager.FindByNameAsync(rolesViewModel.RoleName);
            if (exists is null)
            {
                IdentityRole<int> identityRole = new()
                {
                    Name = rolesViewModel.RoleName
                };
                IdentityResult result = await roleManager.DeleteAsync(identityRole);
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
            else
            {
                ModelState.AddModelError(string.Empty, "This Role Is Already Not Exist");
            }
            return View(rolesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddUsersInRoles()
        {
            UserRoleViewModel userRoleViewModel = new(){
                RoleName = string.Empty,
                UserEmail =string.Empty ,
                AvailableRoles = await roleManager.Roles.Select(element=>element.Name).ToListAsync(),
            };
            return View(userRoleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUsersInRoles(UserRoleViewModel userRoleViewModel)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(userRoleViewModel.UserEmail);
            IdentityRole<int>? role = await roleManager.FindByNameAsync(userRoleViewModel.RoleName);
            if (user is not null && role is not null)
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, userRoleViewModel.RoleName);
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
            return View("ManageUserRoles",userRoleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUsersInRoles()
        {
            UserRoleViewModel userRoleViewModel = new(){
                AvailableRoles = await roleManager.Roles.Select(element=>element.Name).ToListAsync()
            };
            return View("ManageUserRoles",userRoleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUsersInRolesAsync(UserRoleViewModel userRoleViewModel)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(userRoleViewModel.UserEmail);
            IdentityRole<int>? role = await roleManager.FindByNameAsync(userRoleViewModel.RoleName);
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
            return View("ManageUserRoles",userRoleViewModel);
        }

    }
}