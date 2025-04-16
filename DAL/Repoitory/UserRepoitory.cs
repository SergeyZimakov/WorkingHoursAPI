using Core.Entity.Auth;
using Core.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repoitory
{
    public class UserRepoitory : IUserRepository
    {
        private readonly EntityDbContext _db;
        public UserRepoitory(EntityDbContext db)
        {
            _db = db;
        }
        public async Task<UserEntity?> GetByLoginAsync(string login)
        {
            return await _db.User.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task CreateAsync(UserEntity model)
        {
            await _db.User.AddAsync(model);
            await _db.SaveChangesAsync();
        }
    }
}
