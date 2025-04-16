using Core.Entity.Auth;

namespace Core.Interface.Repository
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByLoginAsync(string login);
        Task CreateAsync(UserEntity model);
    }
}
