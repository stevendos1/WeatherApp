using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Models;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using WeatherApp.Shared.Dtos;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoordinatesController : ControllerBase
    {
        private readonly DataContext _context;

        public CoordinatesController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las coordenadas almacenadas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCoordinates()
        {
            try
            {
                var coordinates = await _context.Coordinates.ToListAsync();
                return Ok(coordinates);
            }
            catch
            {
                return StatusCode(500, "Error al obtener las coordenadas.");
            }
        }
    }
}
