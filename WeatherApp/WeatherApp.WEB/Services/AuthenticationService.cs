using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using WeatherApp.Shared.Dtos;

namespace WeatherApp.WEB.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IJSRuntime _jsRuntime;
        private readonly string _tokenKey = "authToken";

        public AuthenticationService(HttpClient httpClient,
                                     AuthenticationStateProvider authenticationStateProvider,
                                     IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Accounts/login", loginDto);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

                    if (result != null && !string.IsNullOrEmpty(result.Token))
                    {
                        // Guardar el token en localStorage
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _tokenKey, result.Token);

                        // Notificar al AuthenticationStateProvider
                        if (_authenticationStateProvider is CustomAuthenticationStateProvider customAuthProvider)
                        {
                            customAuthProvider.MarkUserAsAuthenticated(result.Token);
                        }

                        // Configurar el HttpClient para incluir el token en las solicitudes
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar o registrar la excepción según sea necesario
                Console.Error.WriteLine($"Error en Login: {ex.Message}");
            }

            return false;
        }

        public async Task Logout()
        {
            // Eliminar el token de localStorage
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _tokenKey);

            // Notificar al AuthenticationStateProvider
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

            // Remover el token del HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> Register(UserRegistrationDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Accounts/register", registerDto);
            return response.IsSuccessStatusCode;
        }
    }
}
