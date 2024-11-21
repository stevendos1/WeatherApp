using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Models;
using System.Threading.Tasks;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere autenticación para todos los endpoints
    public class AlertsController : ControllerBase
    {
        private readonly DataContext _context;

        public AlertsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Alerts
        [HttpGet]
        [Authorize(Roles = "Admin,User")] // Permitir acceso a Administradores y Usuarios
        public async Task<IActionResult> GetAlerts()
        {
            var alerts = await _context.Alerts.ToListAsync();
            return Ok(alerts);
        }

        // GET: api/Alerts/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")] // Permitir acceso a Administradores y Usuarios
        public async Task<IActionResult> GetAlertById(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);

            if (alert == null)
                return NotFound("La alerta no fue encontrada.");

            return Ok(alert);
        }

        // POST: api/Alerts
        [HttpPost]
        [Authorize(Roles = "Admin")] // Solo Administradores pueden crear alertas
        public async Task<IActionResult> AddAlert([FromBody] Alert alert)
        {
            if (alert == null)
                return BadRequest("Los datos de la alerta son inválidos.");

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAlertById), new { id = alert.Id }, alert);
        }

        // PUT: api/Alerts/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Solo Administradores pueden actualizar alertas
        public async Task<IActionResult> UpdateAlert(int id, [FromBody] Alert alert)
        {
            if (id != alert.Id)
                return BadRequest("El ID de la alerta no coincide.");

            var existingAlert = await _context.Alerts.FindAsync(id);
            if (existingAlert == null)
                return NotFound("La alerta no fue encontrada.");

            existingAlert.Title = alert.Title;
            existingAlert.Description = alert.Description;
            existingAlert.Date = alert.Date;
            existingAlert.CityId = alert.CityId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Alerts/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Solo Administradores pueden eliminar alertas
        public async Task<IActionResult> DeleteAlert(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert == null)
                return NotFound("La alerta no fue encontrada.");

            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
