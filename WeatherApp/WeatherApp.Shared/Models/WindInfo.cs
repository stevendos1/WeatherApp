namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class WindInfo
    {
        [Key]
        public int Id { get; set; }  // Identificador único del registro de viento

        [Required]
        public double Speed { get; set; }  // Velocidad del viento en km/h

        public double Direction { get; set; }  // Dirección del viento en grados

        public int WeatherInfoId { get; set; }  // Relación uno a uno con WeatherInfo
        public WeatherInfo WeatherInfo { get; set; }
    }
}
