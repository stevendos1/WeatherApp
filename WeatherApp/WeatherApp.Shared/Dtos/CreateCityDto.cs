namespace WeatherApp.Shared.Dtos
{
    public class CreateCityDto
    {
        public string Name { get; set; } // Nombre de la ciudad
        public string Country { get; set; } // País al que pertenece la ciudad
        public double Latitude { get; set; } // Latitud
        public double Longitude { get; set; } // Longitud
    }
}
