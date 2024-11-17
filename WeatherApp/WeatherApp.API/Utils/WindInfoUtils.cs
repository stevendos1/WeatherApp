namespace WeatherApp.API.Utils
{
    public static class WindInfoUtils
    {
        public static string ConvertWindDirectionToText(int direction)
        {
            if (direction == 0) return "Norte";
            if (direction == 90) return "Este";
            if (direction == 180) return "Sur";
            if (direction == 270) return "Oeste";
            return "Dirección desconocida";
        }
    }
}
