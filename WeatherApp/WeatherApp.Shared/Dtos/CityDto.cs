using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Shared.Dtos;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public CoordinatesDto Coordinates { get; set; }
    public List<WeatherInfoDto> WeatherHistory { get; set; }
    public List<ForecastDto> Forecasts { get; set; }
    public List<AlertDto> Alerts { get; set; }
}
