namespace WeatherApp.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WeatherApp.API.Data;
    using WeatherApp.Shared.Dtos;

    [ApiController]
    [Route("api/[controller]")]
    public class WeatherInfoController : ControllerBase
    {
        private readonly DataContext _context;

        public WeatherInfoController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeatherInfoById(int id)
        {
            var weatherInfo = await _context.WeatherInfos
                .Include(w => w.Wind)
                .Include(w => w.CloudCoverage)
                .Include(w => w.SunInfo)
                .Include(w => w.TemperatureDetails)
                .Where(w => w.Id == id)
                .Select(w => new WeatherInfoDto
                {
                    Id = w.Id,
                    Temperature = w.Temperature,
                    Humidity = w.Humidity,
                    Pressure = w.Pressure,
                    Description = w.Description,
                    Wind = w.Wind != null ? new WindInfoDto
                    {
                        Id = w.Wind.Id,
                        Speed = w.Wind.Speed,
                        Direction = w.Wind.Direction.ToString()

                    } : null,
                    CloudCoverage = w.CloudCoverage != null ? new CloudCoverageDto
                    {
                        Id = w.CloudCoverage.Id,
                        Percentage = w.CloudCoverage.Percentage
                    } : null,
                    SunInfo = w.SunInfo != null ? new SunInfoDto
                    {
                        Id = w.SunInfo.Id,
                        Sunrise = w.SunInfo.Sunrise,
                        Sunset = w.SunInfo.Sunset
                    } : null,
                    TemperatureDetails = w.TemperatureDetails != null ? new TemperatureDetailsDto
                    {
                        Id = w.TemperatureDetails.Id,
                        Min = w.TemperatureDetails.Min,
                        Max = w.TemperatureDetails.Max
                    } : null
                })
                .FirstOrDefaultAsync();

            if (weatherInfo == null)
                return NotFound("Weather information not found");

            return Ok(weatherInfo);
        }
    }
}
