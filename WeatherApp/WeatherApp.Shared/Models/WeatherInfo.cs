namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class WeatherInfo
    {
        [Key]
        public int Id { get; set; }  // Identificador único para el registro de clima

        [Required]
        public double Temperature { get; set; }  // Temperatura en grados Celsius

        [Required]
        public double Humidity { get; set; }  // Humedad relativa en porcentaje

        public double Pressure { get; set; }  // Presión atmosférica en hPa

        [MaxLength(100)]
        public string Description { get; set; }  // Descripción del clima (ejemplo: "Soleado")

        public int CityId { get; set; }  // Relación muchos a uno con City
        public City City { get; set; }

        public WindInfo Wind { get; set; }  // Relación uno a uno con WindInfo
        public CloudCoverage CloudCoverage { get; set; }  // Relación uno a uno con CloudCoverage
        public SunInfo SunInfo { get; set; }  // Relación uno a uno con SunInfo
        public TemperatureDetails TemperatureDetails { get; set; }  // Relación uno a uno con TemperatureDetails
    }
}
