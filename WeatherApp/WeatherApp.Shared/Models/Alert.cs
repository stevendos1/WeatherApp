namespace WeatherApp.Shared.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Representa una alerta climática asociada a una ciudad.
    /// </summary>
    public class Alert
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [MinLength(5, ErrorMessage = "El título debe tener al menos 5 caracteres.")]
        [MaxLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres.")]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "El ID de la ciudad es obligatorio.")]
        [ForeignKey(nameof(City))]
        public int CityId { get; set; }

        public City City { get; set; }
    }
}
