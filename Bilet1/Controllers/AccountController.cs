using Bilet1.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bilet1.Controllers;

public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager) : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
    }

    public IActionResult CreateRoles()
    {
        _roleManager.CreateAsync(new() { Name = "Admin" });
        _roleManager.CreateAsync(new() { Name = "Member" });

    }
}
