namespace WeatherApp.Shared.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class City
    {
        [Key]
        public int Id { get; set; }  // Identificador único para la ciudad

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }  // Nombre de la ciudad

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }  // País al que pertenece la ciudad

        public Coordinates Coordinates { get; set; }  // Relación uno a uno con Coordinates

        public List<Forecast> Forecasts { get; set; } = new List<Forecast>();  // Relación uno a muchos con Forecast

        public List<WeatherInfo> WeatherHistory { get; set; } = new List<WeatherInfo>();  // Relación uno a muchos con WeatherInfo

        public List<Alert> Alerts { get; set; } = new List<Alert>();  // Relación uno a muchos con Alert

        // Relación uno a muchos con User
        public List<User> Users { get; set; } = new List<User>();
    }
}
