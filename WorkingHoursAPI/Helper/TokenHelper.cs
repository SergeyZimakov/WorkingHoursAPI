using Core.DTO;
using Core.Entity.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WorkingHoursAPI.Helper
{
    public static class TokenHelper
    {
        private static IConfiguration _configuration;

        public static void GetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static UserDTO GetUser(this ControllerBase controller)
        {
            var idClaim = controller.HttpContext.User.Claims.First(c => c.Type == "ID");
            var LoginClaim = controller.HttpContext.User.Claims.First(c => c.Type == "Login");
            var FirstName = controller.HttpContext.User.Claims.First(c => c.Type == "FirstName");
            var LastName = controller.HttpContext.User.Claims.First(c => c.Type == "LastName");

            return new UserDTO
            {
                UserID = long.Parse(idClaim.Value),
                Login = FirstName.Value,
                FirstName = FirstName.Value,
                LastName = LastName.Value,
            };
        }

        public static string GenerateToken(this ControllerBase controller, UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creadentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Issuer"],
                    claims: new[]
                    {
                        new Claim("ID", user.UserID.ToString()),
                        new Claim("Login", user.Login),
                        new Claim("FirstName", user.FirstName),
                        new Claim("LastName", user.LastName),
                    },
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creadentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
