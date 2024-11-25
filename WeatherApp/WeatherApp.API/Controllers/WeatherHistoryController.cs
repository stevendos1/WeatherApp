using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.API.Data;
using WeatherApp.Shared.Models;
using WeatherApp.Shared.Dtos;

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
                // Consulta para obtener el historial de clima con las relaciones necesarias
                var query = _context.WeatherInfos
                    .Include(w => w.Wind)
                    .Include(w => w.CloudCoverage)
                    .Include(w => w.SunInfo)
                    .Include(w => w.TemperatureDetails)
                    .Include(w => w.City)
                        .ThenInclude(c => c.Coordinates)
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

                // Mapear a DTOs
                var weatherHistoryDtos = weatherHistory.Select(w => MapWeatherInfoToDto(w)).ToList();

                return Ok(weatherHistoryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el historial de clima: {ex.Message}");
            }
        }

        private WeatherInfoDto MapWeatherInfoToDto(WeatherInfo weatherInfo)
        {
            return new WeatherInfoDto
            {
                Id = weatherInfo.Id,
                Date = weatherInfo.Date,
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
