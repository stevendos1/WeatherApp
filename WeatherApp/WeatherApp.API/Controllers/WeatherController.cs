using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherApp.API.Services;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherApiService _weatherApiService;

        /// <summary>
        /// Constructor del controlador WeatherController.
        /// </summary>
        /// <param name="weatherApiService">Servicio para interactuar con la API de clima.</param>
        public WeatherController(WeatherApiService weatherApiService)
        {
            _weatherApiService = weatherApiService;
        }

        /// <summary>
        /// Sincroniza los datos del clima para las coordenadas especificadas.
        /// </summary>
        /// <param name="lat">Latitud de la ubicación (entre -90 y 90).</param>
        /// <param name="lon">Longitud de la ubicación (entre -180 y 180).</param>
        /// <returns>Mensaje de éxito o error.</returns>
        [HttpPost("sync")]
        public async Task<IActionResult> SyncWeatherData([FromQuery] double lat, [FromQuery] double lon)
        {
            if (lat < -90 || lat > 90)
                return BadRequest("La latitud debe estar entre -90 y 90.");
            if (lon < -180 || lon > 180)
                return BadRequest("La longitud debe estar entre -180 y 180.");

            try
            {
                // Llamar directamente al método de sincronización
                await _weatherApiService.SyncWeatherDataAsync(lat, lon);
                return Ok("Datos sincronizados correctamente.");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, $"Error al comunicarse con la API de clima: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

    }
}
