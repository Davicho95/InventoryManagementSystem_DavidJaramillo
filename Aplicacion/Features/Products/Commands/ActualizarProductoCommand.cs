using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Aplicacion.Wrappers;
using MediatR;

namespace Aplicacion.Features.Products.Commands
{
    public class ActualizarProductoCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadEnStock { get; set; }
        public decimal PrecioUnitario { get; set; }

        public class ActualizarProductoHandler : IRequestHandler<ActualizarProductoCommand, Response<bool>>
        {
            private readonly IProductoRepositoryAsync _repositorio;

            public ActualizarProductoHandler(IProductoRepositoryAsync repositorio)
            {
                _repositorio = repositorio;
            }

            public async Task<Response<bool>> Handle(ActualizarProductoCommand request, CancellationToken cancellationToken)
            {
                var producto = await _repositorio.ObtenerPorIdAsync(request.Id)
                    ?? throw new ApiException($"Producto con Id {request.Id} no encontrado.");

                var esUnico = await _repositorio.EsSKUUnicoAsync(request.SKU, request.Id);
                if (!esUnico)
                    throw new ApiException($"El SKU '{request.SKU}' ya está en uso.");

                if (request.CantidadEnStock < 0)
                    throw new ValidationException(new List<string> { "El stock no puede ser negativo." });

                producto.Name = request.Nombre;
                producto.SKU = request.SKU;
                producto.Category = request.Categoria;
                producto.QuantityInStock = request.CantidadEnStock;
                producto.UnitPrice = request.PrecioUnitario;

                await _repositorio.ActualizarAsync(producto);

                return new Response<bool>(true);
            }
        }
    }
}
