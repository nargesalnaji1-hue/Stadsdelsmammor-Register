using StadsdelsmammorPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stadsdelsmammor.Models;
using Stadsdelsmammor.ViewModels;

namespace Stadsdelsmammor.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) &&
                Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction(
                "Index",
                "Mamma");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(
                string.Empty,
                "Kontot är tillfälligt låst. Försök igen senare.");

            return View(model);
        }

        ModelState.AddModelError(
            string.Empty,
            "Felaktig e-postadress eller lösenord.");

        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true,
            FullName = model.FullName
        };

        var result = await _userManager.CreateAsync(
            user,
            model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Admin");

            TempData["SuccessMessage"] =
                "Den nya administratören skapades.";

            return RedirectToAction(
                "Index",
                "Mamma");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(
                string.Empty,
                error.Description);
        }

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(
            "Login",
            "Account");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
