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
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if(!ModelState.IsValid) return View(vm);

        var user = await _userManager.FindByNameAsync(vm.UserName);

        if(user is null)
        {
            ModelState.AddModelError("", "Username or password is wrong");
            return View(vm);
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, vm.Password, false, true);

        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError("", "Email or password is wrong");
            return View(vm);
        }
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        AppUser user = new()
        {
            UserName = vm.UserName,
            Email = vm.Email,
            Country = vm.Country
        };

        var result = await _userManager.CreateAsync(user, vm.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(vm);
        }

        await _userManager.AddToRoleAsync(user, "Member");
        await _signInManager.SignInAsync(user, false);

        return RedirectToAction("Index", "Home");
    }
}
