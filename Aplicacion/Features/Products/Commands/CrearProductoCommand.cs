using Aplicacion.Dto.Product;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Aplicacion.Wrappers;
using Dominio.Entidades;
using MediatR;

namespace Aplicacion.Features.Products.Commands
{
    public class CrearProductoCommand : IRequest<Response<ProductoDto>>
    {
        public string Nombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadEnStock { get; set; }
        public decimal PrecioUnitario { get; set; }

        public class CrearProductoHandler : IRequestHandler<CrearProductoCommand, Response<ProductoDto>>
        {
            private readonly IProductoRepositoryAsync _repositorio;

            public CrearProductoHandler(IProductoRepositoryAsync repositorio)
            {
                _repositorio = repositorio;
            }

            public async Task<Response<ProductoDto>> Handle(CrearProductoCommand request, CancellationToken cancellationToken)
            {
                var esUnico = await _repositorio.EsSKUUnicoAsync(request.SKU);
                if (!esUnico)
                    throw new ApiException($"El SKU '{request.SKU}' ya está en uso.");

                if (request.CantidadEnStock < 0)
                    throw new ValidationException(new List<string> { "El stock inicial no puede ser negativo." });

                var producto = new Product
                {
                    Name = request.Nombre,
                    SKU = request.SKU,
                    Category = request.Categoria,
                    QuantityInStock = request.CantidadEnStock,
                    UnitPrice = request.PrecioUnitario,
                    CreatedAt = DateTime.UtcNow
                };

                await _repositorio.AgregarAsync(producto);

                return new Response<ProductoDto>(new ProductoDto
                {
                    Id = producto.Id,
                    Nombre = producto.Name,
                    SKU = producto.SKU,
                    Categoria = producto.Category,
                    CantidadEnStock = producto.QuantityInStock,
                    PrecioUnitario = producto.UnitPrice,
                    FechaCreacion = producto.CreatedAt
                });
            }
        }
    }
}
