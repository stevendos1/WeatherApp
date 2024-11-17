namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SunInfo
    {
        [Key]
        public int Id { get; set; }  // Identificador único del registro solar

        [Required]
        public DateTime Sunrise { get; set; }  // Hora de salida del sol

        [Required]
        public DateTime Sunset { get; set; }  // Hora de puesta del sol

        public int WeatherInfoId { get; set; }  // Relación uno a uno con WeatherInfo
        public WeatherInfo WeatherInfo { get; set; }
    }
}
