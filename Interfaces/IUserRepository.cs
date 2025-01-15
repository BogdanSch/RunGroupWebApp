using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetAllUsers();
    Task<AppUser> GetUserById(string id);
    Task<AppUser> GetUserByIdNoTracking(string id);
    bool AddUser(AppUser user);
    bool UpdateUser(AppUser user);
    bool DeleteUser(AppUser user);
    bool Save();
}
