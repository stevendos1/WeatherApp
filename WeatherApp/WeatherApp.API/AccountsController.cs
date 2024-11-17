using Microsoft.AspNetCore.Mvc;
using WeatherApp.Shared.Models;
using WeatherApp.Shared.Dtos;  
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using WeatherApp.API.Data;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly DataContext _context;

    public AccountsController(DataContext context)
    {
        _context = context;
    }

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
            PasswordHash = HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("User registered successfully");
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
