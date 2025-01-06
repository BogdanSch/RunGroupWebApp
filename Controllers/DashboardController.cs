using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;
using RunGroupWebApp.ViewModels.User;
using CloudinaryDotNet.Actions;

namespace RunGroupWebApp.Controllers;
public class DashboardController : Controller
{
    private readonly IDashboardRepository _dasboardRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;
    public DashboardController(IDashboardRepository dasboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
    {
        _dasboardRepository = dasboardRepository;
        _httpContextAccessor = httpContextAccessor;
        _photoService = photoService;
    }

    public IActionResult Index()
    {
        List<Race> userRaces = _dasboardRepository.GetAllUserRaces();
        List<Club> userClubs = _dasboardRepository.GetAllUserClubs();

        DashboardViewModel dashboardViewModel = new DashboardViewModel(userRaces, userClubs);
        return View(dashboardViewModel);
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
        AppUser user = await _dasboardRepository.GetUserById(currentUserId);

        if(user == null)
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
        _dasboardRepository.Update(user);
    }
    [HttpPost]
    public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editUserViewModel)
    {
        if(!ModelState.IsValid)
        {
            ModelState.AddModelError("error", "Failed to edit the user profile");
            return View(editUserViewModel);
        }

        AppUser user = await _dasboardRepository.GetUserByIdNoTracking(editUserViewModel.Id);

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
