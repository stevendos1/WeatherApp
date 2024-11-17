using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WeatherApp.API.Data
{
    public static class DataSeeder
    {
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            // Asegurar que la base de datos está migrada
            context.Database.Migrate();

            // Sembrar datos
            SeedRoles(context);
            SeedAdminUser(context);
            SeedCities(context);
            SeedWeatherData(context);

            // Guardar los cambios
            context.SaveChanges();
        }

        private static void SeedRoles(DataContext context)
        {
            // Sembrar roles predeterminados
            var roles = new List<string> { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!context.Users.Any(u => u.Role == role))
                {
                    Console.WriteLine($"Rol predeterminado agregado: {role}");
                }
            }
        }

        private static void SeedAdminUser(DataContext context)
        {
            // Crear usuario administrador si no existe
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = HashPassword("admin123"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                context.Users.Add(adminUser);
                Console.WriteLine("Usuario administrador creado exitosamente.");
            }
        }

        private static void SeedCities(DataContext context)
        {
            // Crear ciudades si no existen
            if (!context.Cities.Any())
            {
                var cities = new List<City>
                {
                    new City
                    {
                        Name = "Medellín",
                        Country = "Colombia",
                        Coordinates = new Coordinates { Latitude = 6.2442, Longitude = -75.5812 },
                        Forecasts = new List<Forecast>
                        {
                            new Forecast { Date = DateTime.UtcNow.AddDays(1), Temperature = 25, Humidity = 70 },
                            new Forecast { Date = DateTime.UtcNow.AddDays(2), Temperature = 27, Humidity = 65 }
                        },
                        Alerts = new List<Alert>
                        {
                            new Alert { Title = "Lluvias fuertes", Description = "Se esperan lluvias intensas en la noche.", Date = DateTime.UtcNow }
                        }
                    },
                    new City
                    {
                        Name = "Bogotá",
                        Country = "Colombia",
                        Coordinates = new Coordinates { Latitude = 4.7110, Longitude = -74.0721 },
                        Forecasts = new List<Forecast>
                        {
                            new Forecast { Date = DateTime.UtcNow.AddDays(1), Temperature = 15, Humidity = 80 },
                            new Forecast { Date = DateTime.UtcNow.AddDays(2), Temperature = 18, Humidity = 75 }
                        },
                        Alerts = new List<Alert>
                        {
                            new Alert { Title = "Vientos fuertes", Description = "Se esperan ráfagas de viento en la tarde.", Date = DateTime.UtcNow }
                        }
                    }
                };

                context.Cities.AddRange(cities);
                Console.WriteLine("Ciudades iniciales creadas.");
            }
        }

        private static void SeedWeatherData(DataContext context)
        {
            // Crear datos climáticos para cada ciudad
            if (!context.WeatherInfos.Any())
            {
                var cities = context.Cities.ToList();
                foreach (var city in cities)
                {
                    var weatherInfo = new WeatherInfo
                    {
                        Temperature = 20 + city.Id, // Temperatura variable según la ciudad
                        Humidity = 60 + city.Id,
                        Pressure = 1010,
                        Description = "Despejado",
                        Wind = new WindInfo { Speed = 10, Direction = 180 },
                        CloudCoverage = new CloudCoverage { Percentage = 20 },
                        SunInfo = new SunInfo { Sunrise = DateTime.UtcNow.AddHours(-6), Sunset = DateTime.UtcNow.AddHours(6) },
                        TemperatureDetails = new TemperatureDetails { Min = 15, Max = 30 },
                        CityId = city.Id
                    };

                    context.WeatherInfos.Add(weatherInfo);
                }
                Console.WriteLine("Datos climáticos iniciales creados para todas las ciudades.");
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
