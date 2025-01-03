﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.Models;

public class AppUser : IdentityUser
{
    public int? Pace { get; set; }
    public int? Mileage { get; set; }
    [ForeignKey(nameof(Address))]
    public int AddressId { get; set; }
    public Address? Address { get; set; }
    //public bool EmailConfirmed { get; set; }
    public ICollection<Club> Clubs { get; set; }
    public ICollection<Race> Races { get; set; }
}