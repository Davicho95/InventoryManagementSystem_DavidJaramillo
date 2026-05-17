using Aplicacion.Dto.StockMovement;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Aplicacion.Wrappers;
using MediatR;

namespace Aplicacion.Features.StockMovements.Queries
{
    public class ObtenerMovimientosPorProductoQuery : IRequest<Response<IReadOnlyList<MovimientoStockDto>>>
    {
        public int ProductoId { get; set; }

        public class ObtenerMovimientosPorProductoHandler : IRequestHandler<ObtenerMovimientosPorProductoQuery, Response<IReadOnlyList<MovimientoStockDto>>>
        {
            private readonly IProductoRepositoryAsync _repositorioProducto;
            private readonly IMovimientoStockRepositoryAsync _repositorioMovimiento;

            public ObtenerMovimientosPorProductoHandler(
                IProductoRepositoryAsync repositorioProducto,
                IMovimientoStockRepositoryAsync repositorioMovimiento)
            {
                _repositorioProducto = repositorioProducto;
                _repositorioMovimiento = repositorioMovimiento;
            }

            public async Task<Response<IReadOnlyList<MovimientoStockDto>>> Handle(ObtenerMovimientosPorProductoQuery request, CancellationToken cancellationToken)
            {
                var producto = await _repositorioProducto.ObtenerPorIdAsync(request.ProductoId)
                    ?? throw new ApiException($"Producto con Id {request.ProductoId} no encontrado.");

                var movimientos = await _repositorioMovimiento.ObtenerPorProductoIdAsync(request.ProductoId);

                var resultado = movimientos.Select(m => new MovimientoStockDto
                {
                    Id = m.Id,
                    ProductoId = m.ProductId,
                    Tipo = m.Type,
                    Cantidad = m.Quantity,
                    Motivo = m.Reason,
                    FechaHora = m.Timestamp
                }).ToList();

                return new Response<IReadOnlyList<MovimientoStockDto>>(resultado);
            }
        }
    }
}
