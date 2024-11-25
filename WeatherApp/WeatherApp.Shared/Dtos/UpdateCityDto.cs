using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Shared.Dtos
{
    public class UpdateCityDto
    {
        public int Id { get; set; } // ID de la ciudad a actualizar
        public string Name { get; set; } // Nombre de la ciudad
        public string Country { get; set; } // País

        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90 grados.")]
        public double Latitude { get; set; } // Latitud

        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180 grados.")]
        public double Longitude { get; set; } // Longitud
    }
}
