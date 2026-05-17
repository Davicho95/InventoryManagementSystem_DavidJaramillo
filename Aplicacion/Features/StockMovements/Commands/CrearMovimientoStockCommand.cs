using Aplicacion.Dto.StockMovement;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Aplicacion.Wrappers;
using Dominio.Entidades;
using Dominio.Enums;
using MediatR;

namespace Aplicacion.Features.StockMovements.Commands
{
    public class CrearMovimientoStockCommand : IRequest<Response<MovimientoStockDto>>
    {
        public int ProductoId { get; set; }
        public MovementType Tipo { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = string.Empty;

        public class CrearMovimientoStockHandler : IRequestHandler<CrearMovimientoStockCommand, Response<MovimientoStockDto>>
        {
            private readonly IProductoRepositoryAsync _repositorioProducto;
            private readonly IMovimientoStockRepositoryAsync _repositorioMovimiento;

            public CrearMovimientoStockHandler(
                IProductoRepositoryAsync repositorioProducto,
                IMovimientoStockRepositoryAsync repositorioMovimiento)
            {
                _repositorioProducto = repositorioProducto;
                _repositorioMovimiento = repositorioMovimiento;
            }

            public async Task<Response<MovimientoStockDto>> Handle(CrearMovimientoStockCommand request, CancellationToken cancellationToken)
            {
                var producto = await _repositorioProducto.ObtenerPorIdAsync(request.ProductoId)
                    ?? throw new ApiException($"Producto con Id {request.ProductoId} no encontrado.");

                // Validación de negocio — no permitir stock negativo
                if (request.Tipo == MovementType.Outbound &&
                    producto.QuantityInStock - request.Cantidad < 0)
                    throw new ValidationException(new List<string> {
                        $"Stock insuficiente. Stock actual: {producto.QuantityInStock}, cantidad solicitada: {request.Cantidad}."
                    });

                // Actualizar stock del producto
                producto.QuantityInStock += request.Tipo == MovementType.Inbound
                    ? request.Cantidad
                    : -request.Cantidad;

                await _repositorioProducto.ActualizarAsync(producto);

                // Registrar el movimiento
                var movimiento = new StockMovement
                {
                    ProductId = request.ProductoId,
                    Type = request.Tipo,
                    Quantity = request.Cantidad,
                    Reason = request.Motivo,
                    Timestamp = DateTime.UtcNow
                };

                await _repositorioMovimiento.AgregarAsync(movimiento);

                return new Response<MovimientoStockDto>(new MovimientoStockDto
                {
                    Id = movimiento.Id,
                    ProductoId = movimiento.ProductId,
                    Tipo = movimiento.Type,
                    Cantidad = movimiento.Quantity,
                    Motivo = movimiento.Reason,
                    FechaHora = movimiento.Timestamp
                });
            }
        }
    }
}
