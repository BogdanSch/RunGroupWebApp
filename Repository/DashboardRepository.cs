using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using System.Security.Claims;

namespace RunGroupWebApp.Repository;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public List<Club> GetAllUserClubs()
    {
        ClaimsPrincipal? currentUser = _httpContextAccessor.HttpContext?.User;
        List<Club> result = _context.Clubs.Where(club => club.AppUserId == currentUser.ToString()).ToList();

        return result;
    }

    public List<Race> GetAllUserRaces()
    {
        ClaimsPrincipal? currentUser = _httpContextAccessor.HttpContext?.User;
        List<Race> result = _context.Races.Where(race => race.AppUserId == currentUser.ToString()).ToList();

        return result;
    }
}
