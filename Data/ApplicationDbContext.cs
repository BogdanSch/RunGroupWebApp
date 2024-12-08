using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) { }
    
    public DbSet<Race> Races { get; set; }
    public DbSet<Club> Clubs { get; set; }  
    public DbSet<Address> Addresses { get; set; }
}
