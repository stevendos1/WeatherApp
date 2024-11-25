using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace WeatherApp.Shared.Dtos;

public class ForecastDto
{
    public int Id { get; set; }  // Agrega esta línea

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public double Temperature { get; set; }

    public double Humidity { get; set; }

    [Required]
    public int CityId { get; set; }
}
