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
    public IActionResult Register()
    {
        RegisterViewModel response = new RegisterViewModel();
        return View(response);
    }
    private Address? FindExistingUserAddress(Address address)
    {
        Address? existingAddress = _context.Addresses.FirstOrDefault(
            a => a.Street == address.Street &&
            a.City == address.City &&
            a.Region == address.Region
        );

        return existingAddress;
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
            return View(registerViewModel);

        AppUser? user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);

        if(user != null)
        {
            TempData["error"] = "Email address already in use. Please, try again";
            return View(registerViewModel);
        }

        Address? existingAddress = FindExistingUserAddress(registerViewModel.Address);

        AppUser newUser = new AppUser
        {
            Email = registerViewModel.EmailAddress,
            UserName = registerViewModel.UserName,
        };

        if(existingAddress != null)
        {
            newUser.Address = existingAddress;
        }
        else
        {
            newUser.Address = registerViewModel.Address;
        }

        IdentityResult userResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

        if (userResponse.Succeeded) 
        {
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        }

        return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
