using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "User Name")]
    [Required(ErrorMessage = "Username is required")]
    [DataType(DataType.Text)]
    public string UserName { get; set; }

    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email address is required")]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirm password")]
    [Required(ErrorMessage = "Password confirmation is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

}