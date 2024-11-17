namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TemperatureDetails
    {
        [Key]
        public int Id { get; set; }  // Identificador único de los detalles de temperatura

        public double Min { get; set; }  // Temperatura mínima registrada o pronosticada

        public double Max { get; set; }  // Temperatura máxima registrada o pronosticada

        public int WeatherInfoId { get; set; }  // Relación uno a uno con WeatherInfo
        public WeatherInfo WeatherInfo { get; set; }
    }
}