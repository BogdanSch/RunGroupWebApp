using RunGroupWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.ViewModels.User;

public class UserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public int Pace { get; set; } = 0;
    public int Mileage { get; set; } = 0;
    public Address Address { get; set; }
    public string ProfileImageUrl { get; set; }
}
