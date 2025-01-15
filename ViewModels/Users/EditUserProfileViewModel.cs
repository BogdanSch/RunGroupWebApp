using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels.Users;

public class EditUserProfileViewModel : UserViewModel
{
    public EditUserProfileViewModel(AppUser appUser)
    {
        Id = appUser.Id;
        UserName = appUser.UserName!;
        Pace = appUser.Pace;
        Mileage = appUser.Mileage;
        Address = appUser.Address!;
        ProfileImageUrl = appUser.ProfileImageUrl;
    }
    public EditUserProfileViewModel() { }
    public IFormFile ProfileImage { get; set; }
}
