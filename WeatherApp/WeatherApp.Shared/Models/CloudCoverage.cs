namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CloudCoverage
    {
        [Key]
        public int Id { get; set; }  // Identificador único de la cobertura de nubes

        public int Percentage { get; set; }  // Porcentaje de cobertura de nubes (0 a 100)

        public int WeatherInfoId { get; set; }  // Relación uno a uno con WeatherInfo
        public WeatherInfo WeatherInfo { get; set; }
    }
}
