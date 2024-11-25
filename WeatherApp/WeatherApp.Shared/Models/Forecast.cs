namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Forecast
    {
        [Key]
        public int Id { get; set; }  // Unique identifier

        [Required]
        public DateTime Date { get; set; }  // Forecast date and time

        [Required]
        public double Temperature { get; set; }  // Forecasted temperature

        public double Humidity { get; set; }  // Forecasted humidity

        [Required]
        public int CityId { get; set; }  // Foreign key to City

        public City? City { get; set; }  // Make City nullable
    }
}
