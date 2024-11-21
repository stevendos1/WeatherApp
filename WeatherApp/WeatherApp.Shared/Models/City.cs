namespace WeatherApp.Shared.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        public Coordinates Coordinates { get; set; } = new Coordinates(); // Inicialización por defecto

        // Propiedades para leer y asignar latitud
        [NotMapped]
        public double Latitude
        {
            get => Coordinates?.Latitude ?? 0; // Retorna 0 si Coordinates es null
            set
            {
                if (Coordinates == null)
                    Coordinates = new Coordinates();
                Coordinates.Latitude = value;
            }
        }

        // Propiedades para leer y asignar longitud
        [NotMapped]
        public double Longitude
        {
            get => Coordinates?.Longitude ?? 0; // Retorna 0 si Coordinates es null
            set
            {
                if (Coordinates == null)
                    Coordinates = new Coordinates();
                Coordinates.Longitude = value;
            }
        }

        public List<Forecast> Forecasts { get; set; } = new List<Forecast>();
        public List<WeatherInfo> WeatherHistory { get; set; } = new List<WeatherInfo>();
        public List<Alert> Alerts { get; set; } = new List<Alert>();
        public List<User> Users { get; set; } = new List<User>();
    }
}
