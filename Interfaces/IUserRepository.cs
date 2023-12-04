using HouseholdOnlineStore.Models;

namespace HouseholdOnlineStore.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<AppUser> GetUsers(string search);
        IEnumerable<AppUser> GetAdmins(string search);
        AppUser GetById(int id);
        AppUser GetByEmail(string email);
        bool CheckIfUserExistsByEmail(string email);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
