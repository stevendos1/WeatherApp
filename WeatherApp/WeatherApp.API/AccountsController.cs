using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WeatherApp.API.Data;
using WeatherApp.Shared.Dtos;
using WeatherApp.Shared.Models;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext _context;

        public AccountsController(DataContext context)
        {
            _context = context;
        }

        // POST: api/Accounts/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest("Username is already taken");

            if (_context.Users.Any(u => u.Email == request.Email))
                return BadRequest("Email is already registered");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = "User", // Default role for new users
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        // POST: api/Accounts/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == login.Username);

            if (user == null || !VerifyPassword(login.Password, user.PasswordHash))
                return Unauthorized("Invalid username or password");

            if (!user.IsActive)
                return Unauthorized("User account is not active");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // Helper: Hash Password
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // Helper: Verify Password
        private bool VerifyPassword(string password, string storedHash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }

        // Helper: Generate JWT Token
        private string GenerateJwtToken(User user)
        {
            // Ensure the key is at least 256 bits (32 characters or more)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourVeryLongSuperSecretKey1234567890"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Reclamos para el token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // Asegúrate de incluir este reclamo
            };


            // Generate the token
            var token = new JwtSecurityToken(
                issuer: "WeatherApp",
                audience: "WeatherAppUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Token expiration
                signingCredentials: creds
            );

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
