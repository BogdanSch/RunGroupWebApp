using Microsoft.AspNetCore.Identity;
using RunGroupWebApp.Data.Enums;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Data;

public class Seed
{
    public static void SeedData(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            // Seed AppUsers first
            if (!context.Set<AppUser>().Any())
            {
                var appUser1 = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),  // Generate unique Id for the user
                    Pace = 5,
                    Mileage = 100,
                    Address = new Address()
                    {
                        Street = "123 Main St",
                        City = "Charlotte",
                        Region = "NC"
                    },
                    EmailConfirmed = true
                };

                var appUser2 = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Pace = 7,
                    Mileage = 150,
                    Address = new Address()
                    {
                        Street = "456 Elm St",
                        City = "Atlanta",
                        Region = "GA"
                    },
                    EmailConfirmed = true
                };

                context.Set<AppUser>().AddRange(appUser1, appUser2);
                context.SaveChanges();  // Save the users before proceeding
            }

            // Get the AppUserIds to assign to Clubs and Races
            var user1Id = context.Set<AppUser>().FirstOrDefault()?.Id;  // Get the first user's Id
            var user2Id = context.Set<AppUser>().Skip(1).FirstOrDefault()?.Id;  // Get the second user's Id

            // Seed Clubs
            if (!context.Clubs.Any())
            {
                context.Clubs.AddRange(new List<Club>()
            {
                new Club()
                {
                    Title = "Running Club 1",
                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                    Description = "This is the description of the first club",
                    ClubCategory = ClubCategory.City,
                    Address = new Address()
                    {
                        Street = "123 Main St",
                        City = "Charlotte",
                        Region = "NC"
                    },
                    AppUserId = user1Id // Assign valid AppUserId
                },
                new Club()
                {
                    Title = "Running Club 2",
                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                    Description = "This is the description of the second club",
                    ClubCategory = ClubCategory.Endurance,
                    Address = new Address()
                    {
                        Street = "456 Elm St",
                        City = "Atlanta",
                        Region = "GA"
                    },
                    AppUserId = user2Id // Assign valid AppUserId
                }
            });
                context.SaveChanges();
            }

            // Seed Races
            if (!context.Races.Any())
            {
                context.Races.AddRange(new List<Race>()
            {
                new Race()
                {
                    Title = "Running Race 1",
                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                    Description = "This is the description of the first race",
                    RaceCategory = RaceCategory.Marathon,
                    Address = new Address()
                    {
                        Street = "123 Main St",
                        City = "Charlotte",
                        Region = "NC"
                    },
                    AppUserId = user1Id // Assign valid AppUserId
                },
                new Race()
                {
                    Title = "Running Race 2",
                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                    Description = "This is the description of the second race",
                    RaceCategory = RaceCategory.Ultra,
                    Address = new Address()
                    {
                        Street = "456 Elm St",
                        City = "Atlanta",
                        Region = "GA"
                    },
                    AppUserId = user2Id // Assign valid AppUserId
                }
            });
                context.SaveChanges();
            }
        }
    }
    public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
    {
        using (IServiceScope serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            //Roles
            RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            //Users
            UserManager<AppUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            string adminUserEmail = "teddysmithdeveloper@gmail.com";

            AppUser adminUser = await userManager.FindByEmailAsync(adminUserEmail);
            if (adminUser == null)
            {
                AppUser newAdminUser = new AppUser()
                {
                    UserName = "teddysmithdev",
                    Email = adminUserEmail,
                    EmailConfirmed = true,
                    Address = new Address()
                    {
                        Street = "123 Main St",
                        City = "Charlotte",
                        Region = "NC"
                    }
                };
                await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
            }

            string appUserEmail = "user@etickets.com";

            AppUser appUser = await userManager.FindByEmailAsync(appUserEmail);
            if (appUser == null)
            {
                AppUser newAppUser = new AppUser()
                {
                    UserName = "app-user",
                    Email = appUserEmail,
                    EmailConfirmed = true,
                    Address = new Address()
                    {
                        Street = "123 Main St",
                        City = "Charlotte",
                        Region = "NC"
                    }
                };
                await userManager.CreateAsync(newAppUser, "Coding@1234?");
                await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
            }
        }
    }
}
