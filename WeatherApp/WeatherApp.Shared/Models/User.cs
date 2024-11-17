namespace WeatherApp.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "User"; // Valor predeterminado: "User"

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsActive { get; set; } = true; // Usuario activo por defecto

        // Relación opcional con City
        public int? CityId { get; set; }
        public City City { get; set; }
    }
}
