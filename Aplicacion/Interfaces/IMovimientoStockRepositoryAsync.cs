using Dominio.Entidades;

namespace Aplicacion.Interfaces
{
    public interface IMovimientoStockRepositoryAsync : IRepositoryAsync<StockMovement>
    {
        Task<IReadOnlyList<StockMovement>> ObtenerPorProductoIdAsync(int productoId);
    }
}
