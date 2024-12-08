using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository;

public class RaceRepository : IRaceRepository
{
    private ApplicationDbContext _context;
    public RaceRepository(ApplicationDbContext context)
    {
        _context = context; 
    }

    public bool Add(Race race)
    {
        _context.Add(race);
        return Save();
    }

    public bool Update(Race race)
    {
        _context.Update(race);
        return Save();
    }

    public bool Delete(Race race)
    {
        _context.Remove(race);
        return Save();
    }
    public bool Save()
    {
        int saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }

    public async Task<IEnumerable<Race>> GetAll()
    {
        return await _context.Races.ToListAsync();
    }

    public async Task<IEnumerable<Race>> GetByCity(string city)
    {
        List<Race> races = await _context.Races.Include(race => race.Address).Where(race => race.Address.City.Contains(city)).ToListAsync();
        return races;
    }

    public async Task<Race> GetByIdAsync(int id)
    {
        return await _context.Races.Include(race => race.Address).FirstOrDefaultAsync(race => race.Id == id);
    }

    public async Task<Race> GetByIdAsyncNoTracking(int id)
    {
        return await _context.Races.Include(race => race.Address).AsNoTracking().FirstOrDefaultAsync(race => race.Id == id);
    }
}
