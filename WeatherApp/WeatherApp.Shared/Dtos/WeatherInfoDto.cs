using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Shared.Dtos;

public class WeatherInfoDto
{
    public int Id { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double Pressure { get; set; }
    public string Description { get; set; }
    public WindInfoDto Wind { get; set; }
    public CloudCoverageDto CloudCoverage { get; set; }
    public SunInfoDto SunInfo { get; set; }
    public TemperatureDetailsDto TemperatureDetails { get; set; }
}
