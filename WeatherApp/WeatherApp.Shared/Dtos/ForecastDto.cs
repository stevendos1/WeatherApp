using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Shared.Dtos;

public class ForecastDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
}
