using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Domain;
using StorageApi.Infrastructure.Repository;

namespace StorageApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {

        private readonly IUserRepository UserRepository;

        public AuthController(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await this.UserRepository.Get(loginRequest.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            throw new Exception("Exception no momento do logout");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create()
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("120997");

            var newUser = new User
            {
                Username = "anapsmendes",
                PasswordHash = passwordHash
            };

            await this.UserRepository.Create(newUser);

            passwordHash = BCrypt.Net.BCrypt.HashPassword("120898");

            newUser = new User
            {
                Username = "leogjorge",
                PasswordHash = passwordHash
            };

            await this.UserRepository.Create(newUser);

            return Ok();

        }
    }

    public record LoginRequest(string Username, string Password);
}
