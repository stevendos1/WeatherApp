using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeatherApp.WEB;
using Microsoft.AspNetCore.Components.Authorization;
using WeatherApp.WEB.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Configurar HttpClient para apuntar a la URL de la API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7054") }); 

// Agregar servicios de autenticación
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();





await builder.Build().RunAsync();
