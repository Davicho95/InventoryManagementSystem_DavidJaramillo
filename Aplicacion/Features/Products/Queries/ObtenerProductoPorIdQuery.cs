using Aplicacion.Dto.Product;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Aplicacion.Wrappers;
using MediatR;

namespace Aplicacion.Features.Products.Queries
{
    public class ObtenerProductoPorIdQuery : IRequest<Response<ProductoDto>>
    {
        public int Id { get; set; }

        public class ObtenerProductoPorIdHandler : IRequestHandler<ObtenerProductoPorIdQuery, Response<ProductoDto>>
        {
            private readonly IProductoRepositoryAsync _repositorio;

            public ObtenerProductoPorIdHandler(IProductoRepositoryAsync repositorio)
            {
                _repositorio = repositorio;
            }

            public async Task<Response<ProductoDto>> Handle(ObtenerProductoPorIdQuery request, CancellationToken cancellationToken)
            {
                var producto = await _repositorio.ObtenerPorIdAsync(request.Id)
                    ?? throw new ApiException($"Producto con Id {request.Id} no encontrado.");

                return new Response<ProductoDto>(new ProductoDto
                {
                    Id = producto.Id,
                    Nombre = producto.Name,
                    SKU = producto.SKU,
                    Categoria = producto.Category,
                    CantidadEnStock = producto.QuantityInStock,
                    PrecioUnitario = producto.UnitPrice,
                    FechaCreacion = producto.CreatedAt,
                    CreatedBy = producto.CreatedBy,
                    UpdatedBy = producto.UpdatedBy
                });
            }
        }
    }
}
