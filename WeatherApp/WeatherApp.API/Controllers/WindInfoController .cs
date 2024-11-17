using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Dtos;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WindInfoController : ControllerBase
    {
        private readonly DataContext _context;

        public WindInfoController(DataContext context)
        {
            _context = context;
        }

        // GET: api/WindInfo/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWindInfoById(int id)
        {
            // Recuperar la información de viento desde la base de datos
            var windInfo = await _context.WindInfos
                .Where(w => w.Id == id)
                .FirstOrDefaultAsync();

            if (windInfo == null)
                return NotFound("Wind information not found");

            // Convertir la información recuperada en el DTO
            var windInfoDto = new WindInfoDto
            {
                Id = windInfo.Id,
                Speed = windInfo.Speed,
                Direction = ConvertWindDirectionToText(windInfo.Direction) // Conversión en memoria
            };

            return Ok(windInfoDto);
        }

        // Método auxiliar: Convertir grados de dirección a texto
        private string ConvertWindDirectionToText(double degrees)
        {
            if ((degrees >= 0 && degrees < 22.5) || (degrees >= 337.5 && degrees <= 360))
                return "N";
            if (degrees >= 22.5 && degrees < 67.5)
                return "NE";
            if (degrees >= 67.5 && degrees < 112.5)
                return "E";
            if (degrees >= 112.5 && degrees < 157.5)
                return "SE";
            if (degrees >= 157.5 && degrees < 202.5)
                return "S";
            if (degrees >= 202.5 && degrees < 247.5)
                return "SW";
            if (degrees >= 247.5 && degrees < 292.5)
                return "W";
            if (degrees >= 292.5 && degrees < 337.5)
                return "NW";

            return "Unknown";
        }
    }
}
