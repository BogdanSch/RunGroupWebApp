using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.Services;
using RunGroupWebApp.ViewModels.Users;

namespace RunGroupWebApp.Controllers;
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;

    public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _photoService = photoService;
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
    private void MapUser(AppUser user, EditUserProfileViewModel editUserViewModel, ImageUploadResult uploadResult)
    {
        user.Id = editUserViewModel.Id;
        user.UserName = editUserViewModel.UserName;
        user.Pace = editUserViewModel.Pace;
        user.Mileage = editUserViewModel.Mileage;
        user.ProfileImageUrl = uploadResult.Url.ToString();
        user.Address = editUserViewModel.Address;
    }
    public async Task<IActionResult> EditUserProfile()
    {
        string currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();
        AppUser user = await _userRepository.GetUserById(currentUserId);

        if (user == null)
        {
            return View("Error");
        }

        EditUserProfileViewModel editUserViewModel = new EditUserProfileViewModel(user);
        return View(editUserViewModel);
    }
    private async Task UpdateUserProfileImage(EditUserProfileViewModel editUserViewModel, AppUser user)
    {
        ImageUploadResult photoResult = await _photoService.AddPhotoAsync(editUserViewModel.ProfileImage);
        MapUser(user, editUserViewModel, photoResult);
        _userRepository.UpdateUser(user);
    }
    [HttpPost]
    public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editUserViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("error", "Failed to edit the user profile");
            return View(editUserViewModel);
        }

        AppUser user = await _userRepository.GetUserByIdNoTracking(editUserViewModel.Id);

        if (string.IsNullOrWhiteSpace(user.ProfileImageUrl))
        {
            await UpdateUserProfileImage(editUserViewModel, user);
            return RedirectToAction("Index");
        }
        try
        {
            await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
        }
        catch (Exception)
        {
            ModelState.AddModelError("error", "Couldn't delete the photo!");
            return View(editUserViewModel);
        }

        await UpdateUserProfileImage(editUserViewModel, user);
        return RedirectToAction("Index");
    }
}
