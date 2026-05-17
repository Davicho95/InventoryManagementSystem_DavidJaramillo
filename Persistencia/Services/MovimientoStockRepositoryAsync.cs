using Aplicacion.Interfaces;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Persistencia.Contexto;

namespace Persistencia.Services
{
    public class MovimientoStockRepositoryAsync : RepositoryAsync<StockMovement>, IMovimientoStockRepositoryAsync
    {
        private readonly AppDbContext _contexto;

        public MovimientoStockRepositoryAsync(AppDbContext contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public async Task<IReadOnlyList<StockMovement>> ObtenerPorProductoIdAsync(int productoId)
        {
            return await _contexto.StockMovements
                .Where(m => m.ProductId == productoId)
                .OrderByDescending(m => m.Timestamp)
                .ToListAsync();
        }
    }
}
