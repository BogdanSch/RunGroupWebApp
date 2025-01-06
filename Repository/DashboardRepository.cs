using Microsoft.EntityFrameworkCore;
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
        string? currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        List<Club> result = _context.Clubs.Where(club => club.AppUserId == currentUserId).ToList();

        return result;
    }

    public List<Race> GetAllUserRaces()
    {
        string? currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        List<Race> result = _context.Races.Where(race => race.AppUserId == currentUserId).ToList();

        return result;
    }
    public async Task<AppUser> GetUserById(string id) => await _context.Users.FindAsync(id);
    public async Task<AppUser> GetUserByIdNoTracking(string id) => await _context.Users.Where(user => user.Id == id).AsNoTracking().FirstOrDefaultAsync();

    public bool Update(AppUser user)
    {
        _context.Users.Update(user);
        return Save();

    }
    public bool Save()
    {
        return _context.SaveChanges() > 0;
    }
}
