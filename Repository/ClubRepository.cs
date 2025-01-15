using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository;

public class ClubRepository : IClubRepository
{
    private readonly ApplicationDbContext _context;
    public ClubRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool Add(Club club)
    {
        _context.Add(club);
        return Save();
    }
    public bool Update(Club club)
    {
        _context.Update(club);
        return Save();
    }

    public bool Delete(Club club)
    {
        _context.Remove(club);
        return Save();
    }
    public bool Save()
    {
        int saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }

    public async Task<IEnumerable<Club>> GetAll()
    {
        return await _context.Clubs.ToListAsync();
    }
    public async Task<IEnumerable<Club>> GetByCity(string city)
    {
        string filteredCity = city.Trim();
        var cityClubs = await _context.Clubs
            .Where(club => club.Address.City != null && club.Address.City.Contains(city.Trim()))
            .Include(club => club.Address)
            .ToListAsync();
        return cityClubs;
    }
    public async Task<Club> GetByIdAsync(int id)
    {
        return await _context.Clubs.Include(club => club.Address).FirstOrDefaultAsync(club => club.Id == id);
    }
    public async Task<Club> GetByIdAsyncNoTracking(int id)
    {
        return await _context.Clubs.Include(club => club.Address).AsNoTracking().FirstOrDefaultAsync(club => club.Id == id);
    }
}
