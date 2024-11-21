using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Shared.Dtos
{
    public class UpdateCityDto
    {
        public int Id { get; set; } // ID de la ciudad a actualizar
        public string Name { get; set; } // Nombre de la ciudad
        public string Country { get; set; } // País
        public double Latitude { get; set; } // Latitud
        public double Longitude { get; set; } // Longitud
    }
}
