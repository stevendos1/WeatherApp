using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly DataContext _context;

        public StatisticsController(DataContext context)
        {
            _context = context; // Inicializar _context con el servicio inyectado
        }

        [HttpGet("City/{cityId}/Statistics")]
        public async Task<IActionResult> GetWeatherStatistics(int cityId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var query = _context.WeatherInfos.Where(w => w.CityId == cityId);

                if (startDate.HasValue)
                    query = query.Where(w => w.SunInfo.Sunrise >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(w => w.SunInfo.Sunset <= endDate.Value);

                if (!await query.AnyAsync())
                {
                    return NotFound("No weather data found for the specified city and date range.");
                }

                var avgTemperature = await query.AverageAsync(w => (double?)w.Temperature) ?? 0.0;
                var avgHumidity = await query.AverageAsync(w => (double?)w.Humidity) ?? 0.0;
                var predominantWind = await query
                    .GroupBy(w => w.Wind.Direction)
                    .OrderByDescending(g => g.Count())
                    .Select(g => (double?)g.Key)
                    .FirstOrDefaultAsync() ?? 0.0;

                var stats = new
                {
                    AvgTemperature = avgTemperature,
                    AvgHumidity = avgHumidity,
                    PredominantWind = predominantWind
                };

                return Ok(stats); // Retornar los datos como respuesta
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.Error.WriteLine($"Error retrieving weather statistics: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving weather statistics.");
            }
        }
    }
}
