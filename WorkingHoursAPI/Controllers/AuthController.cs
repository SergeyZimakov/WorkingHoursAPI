using BLL.Manager;
using Core.DTO;
using Core.View.Auth;
using Microsoft.AspNetCore.Mvc;
using WorkingHoursAPI.Helper;

namespace WorkingHoursAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthManager _manager;
        public AuthController(AuthManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginView requestView)
        {
            var resDto = new CommonContentDTO<TokenView>();
            var userDto = await _manager.LoginAsync(requestView);
            if (userDto == null)
            {
                resDto.Status = CustomResponseStatus.Unauthorized;
                return this.CreateResponse(resDto);
            }

            resDto.Status = CustomResponseStatus.OK;
            resDto.View.Content = new TokenView { Token = this.GenerateToken(userDto) };
            return this.CreateResponse(resDto);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterView requestView)
        {
            var dto = await _manager.RegisterAsync(requestView);
            return this.CreateResponse(dto);
        }
    }
}
