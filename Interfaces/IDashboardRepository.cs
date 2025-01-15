using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces;

public interface IDashboardRepository
{
    List<Race> GetAllUserRaces();
    List<Club> GetAllUserClubs();
}
