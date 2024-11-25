using System.Threading.Tasks;
using WeatherApp.Shared.Dtos;

namespace WeatherApp.WEB.Services
{
    public interface IAuthenticationService
    {
        Task<bool> Login(LoginDto loginDto);
        Task Logout();
        Task<bool> Register(UserRegistrationDto registerDto);
    }
}
