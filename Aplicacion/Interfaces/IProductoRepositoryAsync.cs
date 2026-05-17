using Dominio.Entidades;

namespace Aplicacion.Interfaces
{
    public interface IProductoRepositoryAsync : IRepositoryAsync<Product>
    {
        Task<bool> EsSKUUnicoAsync(string sku, int? excluirId = null);
        Task<IReadOnlyList<Product>> ObtenerFiltradosAsync(string? categoria, int? umbralBajoStock);
    }
}
