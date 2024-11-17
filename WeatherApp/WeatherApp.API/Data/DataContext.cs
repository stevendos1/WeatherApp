using Microsoft.EntityFrameworkCore;
using WeatherApp.Shared.Models;

namespace WeatherApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // DbSets para cada entidad del modelo
        public DbSet<City> Cities { get; set; }
        public DbSet<Coordinates> Coordinates { get; set; }
        public DbSet<WeatherInfo> WeatherInfos { get; set; }
        public DbSet<Forecast> Forecasts { get; set; }
        public DbSet<WindInfo> WindInfos { get; set; }
        public DbSet<CloudCoverage> CloudCoverages { get; set; }
        public DbSet<SunInfo> SunInfos { get; set; }
        public DbSet<TemperatureDetails> TemperatureDetails { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relaciones uno a uno entre City y Coordinates
            modelBuilder.Entity<City>()
                .HasOne(c => c.Coordinates)
                .WithOne(coord => coord.City)
                .HasForeignKey<Coordinates>(coord => coord.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relaciones uno a muchos entre City y Forecast
            modelBuilder.Entity<City>()
                .HasMany(c => c.Forecasts)
                .WithOne(f => f.City)
                .HasForeignKey(f => f.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relaciones uno a muchos entre City y WeatherInfo
            modelBuilder.Entity<City>()
                .HasMany(c => c.WeatherHistory)
                .WithOne(w => w.City)
                .HasForeignKey(w => w.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relaciones uno a muchos entre City y Alert
            modelBuilder.Entity<City>()
                .HasMany(c => c.Alerts)
                .WithOne(a => a.City)
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relaciones uno a uno entre WeatherInfo y WindInfo
            modelBuilder.Entity<WeatherInfo>()
                .HasOne(w => w.Wind)
                .WithOne(wi => wi.WeatherInfo)
                .HasForeignKey<WindInfo>(wi => wi.WeatherInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relaciones uno a uno entre WeatherInfo y CloudCoverage
            modelBuilder.Entity<WeatherInfo>()
                .HasOne(w => w.CloudCoverage)
                .WithOne(cc => cc.WeatherInfo)
                .HasForeignKey<CloudCoverage>(cc => cc.WeatherInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relaciones uno a uno entre WeatherInfo y SunInfo
            modelBuilder.Entity<WeatherInfo>()
                .HasOne(w => w.SunInfo)
                .WithOne(si => si.WeatherInfo)
                .HasForeignKey<SunInfo>(si => si.WeatherInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relaciones uno a uno entre WeatherInfo y TemperatureDetails
            modelBuilder.Entity<WeatherInfo>()
                .HasOne(w => w.TemperatureDetails)
                .WithOne(td => td.WeatherInfo)
                .HasForeignKey<TemperatureDetails>(td => td.WeatherInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relaciones adicionales para User (si aplica)
            // Ejemplo: Relacionar User con City
            modelBuilder.Entity<User>()
                .HasOne(u => u.City)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CityId)
                .OnDelete(DeleteBehavior.SetNull); // En caso de que un usuario pueda no estar relacionado con una ciudad
        }
    }
}
