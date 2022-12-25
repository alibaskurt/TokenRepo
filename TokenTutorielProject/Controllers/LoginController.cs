using Microsoft.AspNetCore.Mvc;
using TokenTutorielProject.Handlers;
using TokenTutorielProject.Models;

namespace TokenTutorielProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            User user = new User
            {
                Id = 1,
                Email = "ali.han.baskurt@gmail.com",
                Name = "Ali",
                Surname = "Başkurt",
                Password = "123"
            };

            bool result = (user.Email == userLoginDto.Email && user.Password == userLoginDto.Password) ? true : false;

            if (!result)
            {
                return NotFound(new { Error = "Email veya şifre alanı yanlış girilmiştir." });
            }
            else
            {
                var tokenHandler = new TokenHandler(_configuration);
                Token token = tokenHandler.CreateAccessToken(user);
                return Ok(new Token { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken, Expiration = token.Expiration });
            }
        }

        [HttpGet]
        public async Task<IActionResult> RefreshTokenLogin([FromForm]string refreshToken)
        {
            //User user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            //if (user != null && user?.RefreshTokenEndDate > DateTime.Now)
            //{
            //    TokenHandler tokenHandler = new TokenHandler(_configuration);
            //    TokenAuthentication token = tokenHandler.CreateAccessToken(user);

            //    user.RefreshToken = token.RefreshToken;
            //    user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
            //    await _context.SaveChangesAsync();

            //    return token;
            //}
            //return null;

            //Veri tabanı bağlantısı olmadığı için postmanda test amaçlı kodları aşağıya yazıyorum.

            //Veri tabanından refresh token ile aşağıdaki user bilgisini çektiğimi varsayyorum.

            User user = new User
            {
                Id = 1,
                Email = "ali.han.baskurt@gmail.com",
                Name = "Ali",
                Surname = "Başkurt",
                Password = "123"
            };

            if (user == null)
            {
                return NotFound(new { Error = "Bu refresh token'a sahip ile kullanıcı bulunamadı" });
            }
            else
            {
                var tokenHandler = new TokenHandler(_configuration);
                var token = tokenHandler.CreateAccessToken(user);
                return Ok(new Token { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken, Expiration = token.Expiration });
            }
        }
    }
}
