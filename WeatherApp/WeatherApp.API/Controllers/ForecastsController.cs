using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Models;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> GetForecasts()
        {
            var forecasts = await _context.Forecasts.ToListAsync();
            return Ok(forecasts);
        }

        [HttpPost]
        public async Task<IActionResult> AddForecast([FromBody] Forecast forecast)
        {
            _context.Forecasts.Add(forecast);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetForecasts), new { id = forecast.Id }, forecast);
        }
    }
}
