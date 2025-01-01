using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RunGroupWebApp.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }


    //By default the GET request
    public IActionResult Login()
    {
        LoginViewModel response = new LoginViewModel();
        return View(response);
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if(!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        AppUser? user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

        if(user != null)
        {
            bool passwordChecked = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (passwordChecked)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded) 
                {
                    return RedirectToAction("Index", nameof(Race));
                }
            }
            TempData["error"] = "Invalid login attempt. Please, try again";
            return View(loginViewModel);
        }
        TempData["error"] = "Invalid credentials. Please, try again";
        return View(loginViewModel);
    }
}
