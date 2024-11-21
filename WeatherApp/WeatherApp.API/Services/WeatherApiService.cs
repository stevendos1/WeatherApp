using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.API.Data;
using WeatherApp.Shared.Models;

namespace WeatherApp.API.Services
{
    public class WeatherApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public WeatherApiService(HttpClient httpClient, IConfiguration configuration, DataContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;

            var baseUrl = _configuration["WeatherApi:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("La BaseUrl para WeatherApi no está configurada en appsettings.json.");
            }
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task SyncWeatherDataAsync(double latitude, double longitude)
        {
            try
            {
                // Obtener la API Key desde appsettings.json
                var apiKey = _configuration["WeatherApi:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new InvalidOperationException("La API Key no está configurada en appsettings.json.");
                }

                // Construir la URL para la API
                var url = $"onecall?lat={latitude}&lon={longitude}&exclude=minutely,hourly,alerts&units=metric&lang=es&appid={apiKey}";

                // Realizar la solicitud GET
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error al obtener datos del clima: {response.StatusCode}, {errorResponse}");
                }

                // Leer la respuesta JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(jsonResponse))
                {
                    throw new InvalidOperationException("La respuesta de la API está vacía.");
                }

                var weatherData = JsonSerializer.Deserialize<JsonElement>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Validar que `weatherData` no sea null
                if (weatherData.ValueKind != JsonValueKind.Object)
                {
                    throw new InvalidOperationException("El formato de la respuesta JSON no es válido.");
                }

                // Transformar y sincronizar datos
                await SaveWeatherDataToDatabase(weatherData, latitude, longitude);
            }
            catch (HttpRequestException ex)
            {
                // Manejar problemas con la solicitud HTTP
                throw new Exception($"Error al comunicarse con la API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                // Manejar errores de deserialización JSON
                throw new Exception($"Error al procesar los datos JSON: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Manejar cualquier otro error
                throw new Exception($"Error inesperado al sincronizar datos del clima: {ex.Message}", ex);
            }
        }

        private async Task SaveWeatherDataToDatabase(JsonElement weatherData, double latitude, double longitude)
        {
            // Validar la estructura del JSON antes de intentar acceder a sus propiedades
            if (!weatherData.TryGetProperty("current", out var current))
            {
                throw new InvalidOperationException("El objeto JSON no contiene la propiedad 'current'.");
            }

            // Validar que 'weather' contenga al menos un elemento
            if (!current.TryGetProperty("weather", out var weatherArray) || weatherArray.ValueKind != JsonValueKind.Array || weatherArray.GetArrayLength() == 0)
            {
                throw new InvalidOperationException("El objeto JSON 'current' no contiene información válida en 'weather'.");
            }

            // Buscar la ciudad por coordenadas usando la relación mapeada de Coordinates
            var city = await _context.Cities
                .Include(c => c.Coordinates) // Incluir la relación para buscar por coordenadas
                .FirstOrDefaultAsync(c => c.Coordinates.Latitude == latitude && c.Coordinates.Longitude == longitude);

            // Si la ciudad no existe, crearla
            if (city == null)
            {
                city = new City
                {
                    Name = $"City_{latitude}_{longitude}",
                    Country = "Unknown", // Asignar un valor predeterminado
                    Coordinates = new Coordinates
                    {
                        Latitude = latitude,
                        Longitude = longitude
                    }
                };

                _context.Cities.Add(city);
            }

            // Crear el WeatherInfo asociado
            var weatherInfo = new WeatherInfo
            {
                Temperature = current.GetProperty("temp").GetDouble(),
                Humidity = current.GetProperty("humidity").GetInt32(),
                Pressure = current.GetProperty("pressure").GetInt32(),
                Description = weatherArray[0].GetProperty("description").GetString(),
                Date = DateTime.UtcNow, // Agregar la fecha actual para el registro
                City = city
            };

            // Agregar el registro de WeatherInfo y guardar los cambios
            _context.WeatherInfos.Add(weatherInfo);
            await _context.SaveChangesAsync();
        }


    }
}
