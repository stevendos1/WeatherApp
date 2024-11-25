using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Dtos;
using WeatherApp.Shared.Models;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public CitiesController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene información detallada de una ciudad por su ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            if (id <= 0)
                return BadRequest("El ID de la ciudad debe ser mayor a 0.");

            try
            {
                var city = await _context.Cities
                    .Include(c => c.Coordinates)
                    .Include(c => c.WeatherHistory)
                    .Include(c => c.Forecasts)
                    .Include(c => c.Alerts)
                    .Where(c => c.Id == id)
                    .Select(c => new CityDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Country = c.Country,
                        Coordinates = c.Coordinates != null
                            ? new CoordinatesDto
                            {
                                Id = c.Coordinates.Id,
                                Latitude = c.Coordinates.Latitude,
                                Longitude = c.Coordinates.Longitude
                            }
                            : null,
                        WeatherHistory = c.WeatherHistory.Select(w => new WeatherInfoDto
                        {
                            Id = w.Id,
                            Temperature = w.Temperature,
                            Humidity = w.Humidity,
                            Pressure = w.Pressure,
                            Description = w.Description
                        }).ToList(),
                        Forecasts = c.Forecasts.Select(f => new ForecastDto
                        {
                            Id = f.Id,
                            Date = f.Date,
                            Temperature = f.Temperature,
                            Humidity = f.Humidity
                        }).ToList(),
                        Alerts = c.Alerts.Select(a => new AlertDto
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Description = a.Description,
                            Date = a.Date
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (city == null)
                    return NotFound("Ciudad no encontrada.");

                return Ok(city);
            }
            catch
            {
                return StatusCode(500, "Error al procesar la solicitud.");
            }
        }

        /// <summary>
        /// Crea una nueva ciudad con los datos proporcionados.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDto createCityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var coordinates = new Coordinates
                {
                    Latitude = createCityDto.Latitude,
                    Longitude = createCityDto.Longitude
                };

                var city = new City
                {
                    Name = createCityDto.Name,
                    Country = createCityDto.Country,
                    Coordinates = coordinates
                };

                _context.Cities.Add(city);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCityById), new { id = city.Id }, new CityDto
                {
                    Id = city.Id,
                    Name = city.Name,
                    Country = city.Country,
                    Coordinates = new CoordinatesDto
                    {
                        Latitude = city.Coordinates.Latitude,
                        Longitude = city.Coordinates.Longitude
                    }
                });
            }
            catch
            {
                return StatusCode(500, "Error al crear la ciudad.");
            }
        }

        /// <summary>
        /// Obtiene una lista de todas las ciudades.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                var cities = await _context.Cities
                    .Include(c => c.Coordinates)
                    .Select(c => new CityDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Country = c.Country,
                        Coordinates = new CoordinatesDto
                        {
                            Latitude = c.Coordinates.Latitude,
                            Longitude = c.Coordinates.Longitude
                        }
                    })
                    .ToListAsync();

                return Ok(cities);
            }
            catch
            {
                return StatusCode(500, "Error al obtener la lista de ciudades.");
            }
        }

        /// <summary>
        /// Actualiza los datos de una ciudad por su ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityDto updateCityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updateCityDto.Id)
                return BadRequest("El ID proporcionado no coincide.");

            try
            {
                var city = await _context.Cities.Include(c => c.Coordinates).FirstOrDefaultAsync(c => c.Id == id);

                if (city == null)
                    return NotFound("Ciudad no encontrada.");

                city.Name = updateCityDto.Name;
                city.Country = updateCityDto.Country;
                city.Coordinates.Latitude = updateCityDto.Latitude;
                city.Coordinates.Longitude = updateCityDto.Longitude;

                await _context.SaveChangesAsync();

                var updatedCityDto = new CityDto
                {
                    Id = city.Id,
                    Name = city.Name,
                    Country = city.Country,
                    Coordinates = new CoordinatesDto
                    {
                        Id = city.Coordinates.Id,
                        Latitude = city.Coordinates.Latitude,
                        Longitude = city.Coordinates.Longitude
                    },
                    WeatherHistory = city.WeatherHistory.Select(w => new WeatherInfoDto
                    {
                        Id = w.Id,
                        Temperature = w.Temperature,
                        Humidity = w.Humidity,
                        Pressure = w.Pressure,
                        Description = w.Description
                    }).ToList(),
                    Forecasts = city.Forecasts.Select(f => new ForecastDto
                    {
                        Id = f.Id,
                        Date = f.Date,
                        Temperature = f.Temperature,
                        Humidity = f.Humidity
                    }).ToList(),
                    Alerts = city.Alerts.Select(a => new AlertDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Description = a.Description,
                        Date = a.Date
                    }).ToList()
                };

                return Ok(updatedCityDto); // Devolver el objeto actualizado con un código de estado 200
            }
            catch
            {
                return StatusCode(500, "Error al actualizar la ciudad.");
            }
        }

        /// <summary>
        /// Elimina una ciudad por su ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                var city = await _context.Cities.Include(c => c.Coordinates).FirstOrDefaultAsync(c => c.Id == id);

                if (city == null)
                    return NotFound("Ciudad no encontrada.");

                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Error al eliminar la ciudad.");
            }
        }
    }
}
