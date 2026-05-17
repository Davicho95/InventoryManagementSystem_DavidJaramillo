using InventoryWeb.Models;
using System.Net.Http.Json;

namespace InventoryWeb.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly JwtAuthStateProvider _authProvider;

        public AuthService(HttpClient http, JwtAuthStateProvider authProvider)
        {
            _http = http;
            _authProvider = (JwtAuthStateProvider)authProvider;
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
            if (result?.Data?.Token == null) return false;

            await _authProvider.NotifyLoginAsync(result.Data.Token);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _authProvider.NotifyLogoutAsync();
        }
    }

    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
    }
}
