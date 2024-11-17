namespace WeatherApp.Shared.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class UserRegistrationDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
