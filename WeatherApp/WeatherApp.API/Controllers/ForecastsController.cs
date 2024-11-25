using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Models;
using System.Threading.Tasks;
using WeatherApp.Shared.Dtos;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForecastsController : ControllerBase
    {
        private readonly DataContext _context;

        public ForecastsController(DataContext context)
        {
            _context = context;
        }

        // Obtener todos los pronósticos
        [HttpGet]
        public async Task<IActionResult> GetForecasts()
        {
            var forecasts = await _context.Forecasts.ToListAsync();
            return Ok(forecasts);
        }

        // Obtener un pronóstico por Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForecastById(int id)
        {
            var forecast = await _context.Forecasts.FindAsync(id);

            if (forecast == null)
                return NotFound();

            return Ok(forecast);
        }

        // Agregar un nuevo pronóstico
        [HttpPost]
        public async Task<IActionResult> AddForecast([FromBody] ForecastDto forecastDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cityExists = await _context.Cities.AnyAsync(c => c.Id == forecastDto.CityId);
            if (!cityExists)
                return BadRequest("El CityId proporcionado no existe.");

            var forecast = new Forecast
            {
                Date = forecastDto.Date,
                Temperature = forecastDto.Temperature,
                Humidity = forecastDto.Humidity,
                CityId = forecastDto.CityId
            };

            _context.Forecasts.Add(forecast);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetForecastById), new { id = forecast.Id }, forecast);
        }
    }
}
