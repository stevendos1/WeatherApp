using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Shared.Dtos;

public class WindInfoDto
{
    public int Id { get; set; }           // Identificador único del registro de viento
    public double Speed { get; set; }    // Velocidad del viento en km/h
    public string Direction { get; set; } // Dirección del viento en formato textual (ejemplo: "N", "SE")
}
