namespace WeatherApp.Shared.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class City
    {
        public City()
        {
            Coordinates = new Coordinates();
            Forecasts = new List<Forecast>();
            WeatherHistory = new List<WeatherInfo>();
            Alerts = new List<Alert>();
            Users = new List<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        public Coordinates Coordinates { get; set; }

        [NotMapped]
        public double Latitude
        {
            get => Coordinates?.Latitude ?? 0;
            set
            {
                if (Coordinates == null)
                    Coordinates = new Coordinates();
                Coordinates.Latitude = value;
            }
        }

        [NotMapped]
        public double Longitude
        {
            get => Coordinates?.Longitude ?? 0;
            set
            {
                if (Coordinates == null)
                    Coordinates = new Coordinates();
                Coordinates.Longitude = value;
            }
        }

        public List<Forecast> Forecasts { get; set; }
        public List<WeatherInfo> WeatherHistory { get; set; }
        public List<Alert> Alerts { get; set; }
        public List<User> Users { get; set; }
    }
}
