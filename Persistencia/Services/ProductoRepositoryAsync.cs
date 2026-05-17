using Aplicacion.Interfaces;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Persistencia.Contexto;

namespace Persistencia.Services
{
    public class ProductoRepositoryAsync : RepositoryAsync<Product>, IProductoRepositoryAsync
    {
        private readonly AppDbContext _contexto;

        public ProductoRepositoryAsync(AppDbContext contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> EsSKUUnicoAsync(string sku, int? excluirId = null)
        {
            return !await _contexto.Products
                .AnyAsync(p => p.SKU == sku && (!excluirId.HasValue || p.Id != excluirId.Value));
        }

        public async Task<IReadOnlyList<Product>> ObtenerFiltradosAsync(string? categoria, int? umbralBajoStock)
        {
            var query = _contexto.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Category == categoria);

            if (umbralBajoStock.HasValue)
                query = query.Where(p => p.QuantityInStock <= umbralBajoStock.Value);

            return await query.OrderBy(p => p.Name).ToListAsync();
        }
    }
}
