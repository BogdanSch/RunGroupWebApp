using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.Models;

public class AppUser : IdentityUser
{
    public int Pace { get; set; } = 0;
    public int Mileage { get; set; } = 0;
    [ForeignKey(nameof(Address))]
    public int? AddressId { get; set; }
    public Address? Address { get; set; }
    public ICollection<Club> Clubs { get; set; }
    public ICollection<Race> Races { get; set; }
    public string ProfileImageUrl { get; set; } = "https://img.freepik.com/free-vector/blue-circle-with-white-user_78370-4707.jpg";
}