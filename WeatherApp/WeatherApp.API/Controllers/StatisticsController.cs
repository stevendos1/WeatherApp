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
            var query = _context.WeatherInfos.Where(w => w.CityId == cityId);

            if (startDate.HasValue)
                query = query.Where(w => w.SunInfo.Sunrise >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(w => w.SunInfo.Sunset <= endDate.Value);

            var stats = new
            {
                AvgTemperature = await query.AverageAsync(w => w.Temperature),
                AvgHumidity = await query.AverageAsync(w => w.Humidity),
                PredominantWind = await query
                    .GroupBy(w => w.Wind.Direction)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync()
            };

            return Ok(stats); // Retornar los datos como respuesta
        }
    }
}
