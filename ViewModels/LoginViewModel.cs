using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email address is required")]
    [Display(Name = "Email Address")]
    public string EmailAddress { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; } = false;

}
