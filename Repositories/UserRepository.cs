using HouseholdOnlineStore.Data;
using HouseholdOnlineStore.Interfaces;
using HouseholdOnlineStore.Models;

namespace HouseholdOnlineStore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;

        public UserRepository(AppDBContext context)
        {
            _context = context;
        }

        public IEnumerable<AppUser> GetUsers(string search)
        {
			var list = _context.Users.AsQueryable();
			if (!string.IsNullOrEmpty(search))
			{
				list = list.Where(u => u.Email.Contains(search));
			}

			return list.Where(u => u.Role == "User").ToList();
        }

        public IEnumerable<AppUser> GetAdmins(string search)
        {
            var list = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(u => u.Email.Contains(search));
            }


            return list.Where(u => u.Role == "Admin").ToList();
        }
        public bool Add(AppUser user)
        {
            _context.Users.Add(user);
            return Save();
        }

        public bool CheckIfUserExistsByEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool Delete(AppUser user)
        {
            _context.Users.Remove(user);
            return Save();
        }

        public AppUser GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }

        public AppUser GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }
    }
}
