using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool AddUser(AppUser user)
    {
        throw new NotImplementedException();
    }

    public bool DeleteUser(AppUser user)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<AppUser>> GetAllUsers() => await _context.Users.ToListAsync();

    public async Task<AppUser> GetUserById(string id) => await _context.Users.FindAsync(id);

    public bool Save()
    {
        bool saved = _context.SaveChanges() > 0;
        return saved;
    }

    public bool UpdateUser(AppUser user)
    {
        _context.Update(user);
        return Save();
    }
}
