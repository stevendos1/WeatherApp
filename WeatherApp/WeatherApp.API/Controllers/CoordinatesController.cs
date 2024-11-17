using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Models;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> GetCoordinates()
        {
            var coordinates = await _context.Coordinates.ToListAsync();
            return Ok(coordinates);
        }

        [HttpPost]
        public async Task<IActionResult> AddCoordinates([FromBody] Coordinates coordinates)
        {
            _context.Coordinates.Add(coordinates);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCoordinates), new { id = coordinates.Id }, coordinates);
        }
    }
}
