using InventoryWeb.Models;
using System.Net.Http.Json;

namespace InventoryWeb.Services
{
    public class ProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http) => _http = http;

        public async Task<List<ProductoDto>> GetAllAsync(string? categoria = null, int? umbralBajoStock = null)
        {
            var url = "api/products";
            var params_ = new List<string>();
            if (!string.IsNullOrEmpty(categoria)) params_.Add($"categoria={categoria}");
            if (umbralBajoStock.HasValue) params_.Add($"umbralBajoStock={umbralBajoStock}");
            if (params_.Any()) url += "?" + string.Join("&", params_);

            var result = await _http.GetFromJsonAsync<ApiResponse<List<ProductoDto>>>(url);
            return result?.Data ?? new();
        }

        public async Task<ProductoDto?> GetByIdAsync(int id)
        {
            var result = await _http.GetFromJsonAsync<ApiResponse<ProductoDto>>($"api/products/{id}");
            return result?.Data;
        }

        public async Task<bool> CreateAsync(CrearProductoDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/products", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, CrearProductoDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/products/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/products/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<MovimientoStockDto>> GetMovementsAsync(int productId)
        {
            var result = await _http.GetFromJsonAsync<ApiResponse<List<MovimientoStockDto>>>($"api/products/{productId}/movements");
            return result?.Data ?? new();
        }

        public async Task<bool> RegisterMovementAsync(int productId, CrearMovimientoDto dto)
        {
            var response = await _http.PostAsJsonAsync($"api/products/{productId}/movements", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
