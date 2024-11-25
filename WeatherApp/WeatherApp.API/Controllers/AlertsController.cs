using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Models;
using WeatherApp.Shared.Dtos;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertsController : ControllerBase
    {
        private readonly DataContext _context;

        public AlertsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Alerts
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAlerts()
        {
            try
            {
                var alerts = await _context.Alerts
                    .Include(a => a.City)
                    .ToListAsync();

                var alertDtos = alerts.Select(a => new AlertDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Date = a.Date,
                    CityId = a.CityId,
                    City = new CityDto
                    {
                        Id = a.City.Id,
                        Name = a.City.Name,
                        Country = a.City.Country,
                        Coordinates = new CoordinatesDto
                        {
                            Id = a.City.Coordinates.Id,
                            Latitude = a.City.Coordinates.Latitude,
                            Longitude = a.City.Coordinates.Longitude
                        }
                    }
                }).ToList();

                return Ok(alertDtos);
            }
            catch
            {
                return StatusCode(500, "Error al obtener las alertas.");
            }
        }

        // GET: api/Alerts/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAlertById(int id)
        {
            try
            {
                var alert = await _context.Alerts
                    .Include(a => a.City)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (alert == null)
                    return NotFound("La alerta no fue encontrada.");

                var alertDto = new AlertDto
                {
                    Id = alert.Id,
                    Title = alert.Title,
                    Description = alert.Description,
                    Date = alert.Date,
                    CityId = alert.CityId,
                    City = new CityDto
                    {
                        Id = alert.City.Id,
                        Name = alert.City.Name,
                        Country = alert.City.Country,
                        Coordinates = new CoordinatesDto
                        {
                            Id = alert.City.Coordinates.Id,
                            Latitude = alert.City.Coordinates.Latitude,
                            Longitude = alert.City.Coordinates.Longitude
                        }
                    }
                };

                return Ok(alertDto);
            }
            catch
            {
                return StatusCode(500, "Error al obtener la alerta.");
            }
        }

        // POST: api/Alerts
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAlert([FromBody] AlertDto alertDto)
        {
            if (alertDto == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            
            if (alertDto.Date < new DateTime(2024, 1, 1))
            {
                return BadRequest(new { Errors = new List<string> { "La fecha de la alerta debe ser a partir del 1 de enero de 2024." } });
            }

            try
            {
                var alert = new Alert
                {
                    Title = alertDto.Title,
                    Description = alertDto.Description,
                    Date = alertDto.Date.ToUniversalTime(), 
                    CityId = alertDto.CityId
                };

                await _context.Alerts.AddAsync(alert);
                await _context.SaveChangesAsync();

                // Actualizar el DTO con el ID generado y la fecha en UTC
                alertDto.Id = alert.Id;
                alertDto.Date = alert.Date;

                return CreatedAtAction(nameof(GetAlertById), new { id = alert.Id }, alertDto);
            }
            catch (Exception ex)
            {
                // Loguear el error si es necesario
                return StatusCode(500, $"Error al crear la alerta: {ex.Message}");
            }
        }

        // PUT: api/Alerts/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAlert(int id, [FromBody] AlertDto alertDto)
        {
            if (id != alertDto.Id)
                return BadRequest("El ID de la alerta no coincide.");

            if (!ModelState.IsValid)
            {
                // Registrar los errores de validación
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Errors = errors });
            }

            // Validar que la fecha sea a partir de 1 de enero de 2024
            if (alertDto.Date < new DateTime(2024, 1, 1))
            {
                return BadRequest(new { Errors = new List<string> { "La fecha de la alerta debe ser a partir del 1 de enero de 2024." } });
            }

            try
            {
                var existingAlert = await _context.Alerts.FindAsync(id);
                if (existingAlert == null)
                    return NotFound("La alerta no fue encontrada.");

                existingAlert.Title = alertDto.Title;
                existingAlert.Description = alertDto.Description;
                existingAlert.Date = alertDto.Date.ToUniversalTime(); 
                existingAlert.CityId = alertDto.CityId;

                await _context.SaveChangesAsync();

                // Actualizar el DTO con la fecha en UTC
                alertDto.Date = existingAlert.Date;

                return Ok(alertDto); 
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, $"Error al actualizar la alerta: {ex.Message}");
            }
        }

        // DELETE: api/Alerts/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            try
            {
                var alert = await _context.Alerts.FindAsync(id);
                if (alert == null)
                    return NotFound("La alerta no fue encontrada.");

                _context.Alerts.Remove(alert);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                // Loguear el error si es necesario
                return StatusCode(500, $"Error al eliminar la alerta: {ex.Message}");
            }
        }
    }
}
