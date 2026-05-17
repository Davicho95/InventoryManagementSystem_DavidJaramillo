using Aplicacion.Dto.Product;
using Aplicacion.Interfaces;
using Aplicacion.Wrappers;
using MediatR;

namespace Aplicacion.Features.Products.Queries
{
    public class ObtenerTodosProductosQuery : IRequest<Response<IReadOnlyList<ProductoDto>>>
    {
        public string? Categoria { get; set; }
        public int? UmbralBajoStock { get; set; }

        public class ObtenerTodosProductosHandler : IRequestHandler<ObtenerTodosProductosQuery, Response<IReadOnlyList<ProductoDto>>>
        {
            private readonly IProductoRepositoryAsync _repositorio;

            public ObtenerTodosProductosHandler(IProductoRepositoryAsync repositorio)
            {
                _repositorio = repositorio;
            }

            public async Task<Response<IReadOnlyList<ProductoDto>>> Handle(ObtenerTodosProductosQuery request, CancellationToken cancellationToken)
            {
                var productos = await _repositorio.ObtenerFiltradosAsync(request.Categoria, request.UmbralBajoStock);

                var resultado = productos.Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Name,
                    SKU = p.SKU,
                    Categoria = p.Category,
                    CantidadEnStock = p.QuantityInStock,
                    PrecioUnitario = p.UnitPrice,
                    FechaCreacion = p.CreatedAt,
                    CreatedBy = p.CreatedBy,
                    UpdatedBy = p.UpdatedBy
                }).ToList();

                return new Response<IReadOnlyList<ProductoDto>>(resultado);
            }
        }
    }
}
