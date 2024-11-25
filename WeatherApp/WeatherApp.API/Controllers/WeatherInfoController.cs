using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Dtos;
using WeatherApp.Shared.Models;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherInfoController : ControllerBase
    {
        private readonly DataContext _context;

        public WeatherInfoController(DataContext context)
        {
            _context = context;
        }

        // GET: api/weatherinfo?lat={lat}&lon={lon}
        [HttpGet]
        public async Task<IActionResult> GetWeatherInfoByCoordinates([FromQuery] double lat, [FromQuery] double lon)
        {
            if (lat < -90 || lat > 90)
                return BadRequest("La latitud debe estar entre -90 y 90.");
            if (lon < -180 || lon > 180)
                return BadRequest("La longitud debe estar entre -180 y 180.");

            var weatherInfo = await _context.WeatherInfos
                .Include(w => w.Wind)
                .Include(w => w.CloudCoverage)
                .Include(w => w.SunInfo)
                .Include(w => w.TemperatureDetails)
                .Include(w => w.City)
                    .ThenInclude(c => c.Coordinates)
                .Where(w => w.City.Coordinates.Latitude == lat && w.City.Coordinates.Longitude == lon)
                .OrderByDescending(w => w.Date)
                .FirstOrDefaultAsync();

            if (weatherInfo == null)
                return NotFound("Información del clima no encontrada.");

            var weatherInfoDto = MapWeatherInfoToDto(weatherInfo);
            return Ok(weatherInfoDto);
        }

        // GET: api/weatherinfo/city/{cityName}
        [HttpGet("city/{cityName}")]
        public async Task<IActionResult> GetWeatherInfoByCityName(string cityName)
        {
            var weatherInfo = await _context.WeatherInfos
                .Include(w => w.Wind)
                .Include(w => w.CloudCoverage)
                .Include(w => w.SunInfo)
                .Include(w => w.TemperatureDetails)
                .Include(w => w.City)
                    .ThenInclude(c => c.Coordinates)
                .Where(w => w.City.Name == cityName)
                .OrderByDescending(w => w.Date)
                .FirstOrDefaultAsync();

            if (weatherInfo == null)
                return NotFound("Información del clima no encontrada.");

            var weatherInfoDto = MapWeatherInfoToDto(weatherInfo);
            return Ok(weatherInfoDto);
        }

        private WeatherInfoDto MapWeatherInfoToDto(WeatherInfo weatherInfo)
        {
            return new WeatherInfoDto
            {
                Id = weatherInfo.Id,
                Date = weatherInfo.Date, // Asignamos la fecha
                Temperature = weatherInfo.Temperature,
                Humidity = weatherInfo.Humidity,
                Pressure = weatherInfo.Pressure,
                Description = weatherInfo.Description,
                CityName = weatherInfo.City.Name,
                Wind = weatherInfo.Wind != null ? new WindInfoDto
                {
                    Id = weatherInfo.Wind.Id,
                    Speed = weatherInfo.Wind.Speed,
                    Direction = ConvertWindDirectionToText(weatherInfo.Wind.Direction)
                } : null,
                CloudCoverage = weatherInfo.CloudCoverage != null ? new CloudCoverageDto
                {
                    Id = weatherInfo.CloudCoverage.Id,
                    Percentage = weatherInfo.CloudCoverage.Percentage
                } : null,
                SunInfo = weatherInfo.SunInfo != null ? new SunInfoDto
                {
                    Id = weatherInfo.SunInfo.Id,
                    Sunrise = weatherInfo.SunInfo.Sunrise,
                    Sunset = weatherInfo.SunInfo.Sunset
                } : null,
                TemperatureDetails = weatherInfo.TemperatureDetails != null ? new TemperatureDetailsDto
                {
                    Id = weatherInfo.TemperatureDetails.Id,
                    Min = weatherInfo.TemperatureDetails.Min,
                    Max = weatherInfo.TemperatureDetails.Max
                } : null
            };
        }


        private string ConvertWindDirectionToText(double degrees)
        {
            string[] directions = { "Norte", "Nornoreste", "Noreste", "Estenoreste", "Este", "Estesureste", "Sureste", "Sursureste", "Sur", "Sursuroeste", "Suroeste", "Oestesuroeste", "Oeste", "Oestenoroeste", "Noroeste", "Nornoroeste", "Norte" };
            int index = (int)((degrees + 11.25) / 22.5);
            return directions[index % 16];
        }
    }
}
