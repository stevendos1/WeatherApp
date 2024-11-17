namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Alert
    {
        [Key]
        public int Id { get; set; }  // Identificador único de la alerta

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }  // Título de la alerta

        [MaxLength(500)]
        public string Description { get; set; }  // Descripción de la alerta

        [Required]
        public DateTime Date { get; set; }  // Fecha de la alerta

        public int CityId { get; set; }  // Relación muchos a uno con City
        public City City { get; set; }
    }

}