using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.API.Data;
using WeatherApp.Shared.Models;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherHistoryController : ControllerBase
    {
        private readonly DataContext _context;

        public WeatherHistoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("City/{cityId}")]
        public async Task<IActionResult> GetWeatherHistoryByCity(int cityId, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (cityId <= 0)
                return BadRequest("El ID de la ciudad debe ser mayor a 0.");

            try
            {
                // Consulta básica para obtener el historial de clima
                var query = _context.WeatherInfos
                    .Include(w => w.City) // Incluye la ciudad asociada
                    .Where(w => w.CityId == cityId);

                // Filtrar por rango de fechas si las fechas son proporcionadas
                if (startDate.HasValue)
                    query = query.Where(w => w.Date >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(w => w.Date <= endDate.Value);

                // Ejecutar la consulta
                var weatherHistory = await query.ToListAsync();

                if (!weatherHistory.Any())
                    return NotFound("No se encontró historial de clima para la ciudad especificada.");

                return Ok(weatherHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el historial de clima: {ex.Message}");
            }
        }
    }
}
