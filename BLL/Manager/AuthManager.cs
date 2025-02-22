using AutoMapper;
using Core.Consts;
using Core.DTO;
using Core.Entity.Auth;
using Core.View.Auth;
using DAL;
using DAL.Repoitory;

namespace BLL.Manager
{
    public class AuthManager
    {
        private readonly UserRepoitory _userRepoitory;
        private readonly IMapper _mapper;
        public AuthManager(EntityDbContext dbContext, IMapper mapper)
        {
            _userRepoitory = new UserRepoitory(dbContext);
            _mapper = mapper;
        }
        public async Task<UserDTO?> LoginAsync(LoginView requestView)
        {
            var user = await _userRepoitory.GetByLoginAsync(requestView.Login);
            if (user == null || !BCrypt.Net.BCrypt.Verify(requestView.Password, user.Password)) return null;

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<CommonDTO> RegisterAsync(RegisterView requestView)
        {
            var res = new CommonDTO();
            var isUserExists = await _userRepoitory.GetByLoginAsync(requestView.Login) != null;
            if (isUserExists)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = AuthConsts.LoginAlreadyExists;
                return res;
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var newUser = new UserEntity
            {
                FirstName = requestView.FirstName,
                LastName = requestView.LastName,
                Login = requestView.Login,
                Password = BCrypt.Net.BCrypt.HashPassword(requestView.Password, salt),
            };

            await _userRepoitory.CreateAsync(newUser);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = AuthConsts.RegisterSuccess;
            return res;
        }
    }
}
