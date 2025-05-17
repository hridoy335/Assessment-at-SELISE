using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SelisejobtestAPI.Common;
using SoftInterface.DLL;
using SoftModels.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SelisejobtestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginController : Controller
    {
        private readonly IUserLogin _userLogin;
        private IConfiguration _config;
        public UserLoginController(IUserLogin userLogin, IConfiguration config)
        {
            _userLogin = userLogin;
            _config = config;
        }

        [HttpGet]
        [Route("LoginUser")]
        public IActionResult LoginUser(string username, string password)
        {
            bool IsSuccess = false;
            string Message = "Wrong username or password! ";
            IActionResult response = Unauthorized();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                UserInfo data = _userLogin.CheckUserLogin(username, password);

                if (data != null)
                {
                    string tokenKey = GenerateToken(data);
                    SetAccessToken(tokenKey);
                    IsSuccess = true;
                    Message = "Login Success...";

                   response = Ok(new { Token = tokenKey, Message = Message, IsSuccess = IsSuccess });
                }
                else
                    response = Ok(new { Message = Message, IsSuccess = IsSuccess });
            }
            else
                return Ok(new { IsSuccess = false, Message = "Login Faild!!" });
            return Ok(response);
        }

        //Create token
        private string GenerateToken(UserInfo obj)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("FullName", obj.FullName),
                new Claim("Id", obj.Id.ToString()),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
             expires: DateTime.Now.AddMinutes(50),
             signingCredentials: credentials
             );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private void SetAccessToken(string tokenKey)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            };
            Response.Cookies.Append("accesstoken", tokenKey, cookieOptions);

        }
    }
}
