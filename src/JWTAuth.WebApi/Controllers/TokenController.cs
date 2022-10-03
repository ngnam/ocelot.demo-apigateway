using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuth.WebApi.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private static readonly UserInfo[] Users = new UserInfo[]
        {
            new UserInfo { UserName = "admin", Password = "admin" },
            new UserInfo { UserName = "user", Password = "user" },
        };

        private readonly ILogger<TokenController> _logger;

        public TokenController(ILogger<TokenController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo _userData)
        {
            if (_userData != null && _userData.UserName != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.UserName, _userData.Password);

                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserName", user.UserName ?? "")
                    };

                    var secret = "Thisismyhtestprivatekey";
                    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: null,
                        audience: null,
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);  ;

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<UserInfo> GetUser(string userName, string passWord)
        {
            await Task.Delay(1000);
            return Users.FirstOrDefault(u => u.UserName == userName && u.Password == passWord) ?? default!;
        }
    }
}