namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Coordinates
    {
        [Key]
        public int Id { get; set; }  // Identificador único para las coordenadas

        [Required]
        public double Latitude { get; set; }  // Latitud de la ciudad

        [Required]
        public double Longitude { get; set; }  // Longitud de la ciudad

        public int CityId { get; set; }  // Relación uno a uno con City
        public City City { get; set; }
    }
}
