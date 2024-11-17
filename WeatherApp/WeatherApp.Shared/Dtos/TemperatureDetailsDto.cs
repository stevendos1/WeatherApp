using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Shared.Dtos;

public class TemperatureDetailsDto
{
    public int Id { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}
