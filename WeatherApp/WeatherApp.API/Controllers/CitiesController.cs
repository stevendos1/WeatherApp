using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Dtos;
namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public CitiesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var city = await _context.Cities
                .Include(c => c.Coordinates)
                .Include(c => c.WeatherHistory)
                .Include(c => c.Forecasts)
                .Include(c => c.Alerts)
                .Where(c => c.Id == id)
                .Select(c => new CityDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Country = c.Country,
                    Coordinates = new CoordinatesDto
                    {
                        Id = c.Coordinates.Id,
                        Latitude = c.Coordinates.Latitude,
                        Longitude = c.Coordinates.Longitude
                    },
                    WeatherHistory = c.WeatherHistory.Select(w => new WeatherInfoDto
                    {
                        Id = w.Id,
                        Temperature = w.Temperature,
                        Humidity = w.Humidity,
                        Pressure = w.Pressure,
                        Description = w.Description
                    }).ToList(),
                    Forecasts = c.Forecasts.Select(f => new ForecastDto
                    {
                        Id = f.Id,
                        Date = f.Date,
                        Temperature = f.Temperature,
                        Humidity = f.Humidity
                    }).ToList(),
                    Alerts = c.Alerts.Select(a => new AlertDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Description = a.Description,
                        Date = a.Date
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (city == null)
                return NotFound("City not found");

            return Ok(city);
        }
    }
}
