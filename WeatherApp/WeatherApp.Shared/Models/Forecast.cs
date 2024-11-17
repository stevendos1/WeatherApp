namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Forecast
    {
        [Key]
        public int Id { get; set; }  // Identificador único del pronóstico

        [Required]
        public DateTime Date { get; set; }  // Fecha y hora del pronóstico

        [Required]
        public double Temperature { get; set; }  // Temperatura pronosticada

        public double Humidity { get; set; }  // Humedad pronosticada

        public int CityId { get; set; }  // Relación muchos a uno con City
        public City City { get; set; }
    }
}