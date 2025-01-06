using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces;

public interface IDashboardRepository
{
    List<Race> GetAllUserRaces();
    List<Club> GetAllUserClubs();
    Task<AppUser> GetUserById(string id);
    Task<AppUser> GetUserByIdNoTracking(string id);
    bool Update(AppUser user);
    bool Save();
}
