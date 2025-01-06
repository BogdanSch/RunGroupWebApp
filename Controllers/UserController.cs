using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels.User;

namespace RunGroupWebApp.Controllers;
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("users")]
    public async Task<IActionResult> Index()
    {
        IEnumerable<AppUser> users = await _userRepository.GetAllUsers();
        List<UserViewModel> userViewModels = new();

        foreach (AppUser appUser in users)
        {
            UserViewModel userViewModel = new()
            {
                Id = appUser.Id,
                UserName = appUser.UserName!,
                Pace = appUser.Pace,
                Mileage = appUser.Mileage,
                ProfileImageUrl = appUser.ProfileImageUrl,
            };
            userViewModels.Add(userViewModel);
        }

        return View(userViewModels);
    }
    public async Task<IActionResult> Detail(string id)
    {
        AppUser appUser = await _userRepository.GetUserById(id);

        UserDetailViewModel userViewModel = new()
        {
            Id = appUser.Id,
            UserName = appUser.UserName!,
            Pace = appUser.Pace,
            Mileage = appUser.Mileage,
            ProfileImageUrl = appUser.ProfileImageUrl,
        };

        return View(userViewModel);
    }
}
