using Aplicacion.Features.Products.Commands;
using Aplicacion.Features.Products.Queries;
using Aplicacion.Features.StockMovements.Commands;
using Aplicacion.Features.StockMovements.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [Authorize]
    public class ProductsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/productos?categoria=Electronics&umbralBajoStock=10
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos(
            [FromQuery] string? categoria,
            [FromQuery] int? umbralBajoStock)
        {
            var resultado = await _mediator.Send(new ObtenerTodosProductosQuery
            {
                Categoria = categoria,
                UmbralBajoStock = umbralBajoStock
            });
            return Ok(resultado);
        }

        // GET /api/productos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _mediator.Send(new ObtenerProductoPorIdQuery { Id = id });
            return Ok(resultado);
        }

        // POST /api/productos
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearProductoCommand command)
        {
            var resultado = await _mediator.Send(command);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Data?.Id }, resultado);
        }

        // PUT /api/productos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProductoCommand command)
        {
            command.Id = id;
            var resultado = await _mediator.Send(command);
            return Ok(resultado);
        }

        // DELETE /api/productos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _mediator.Send(new EliminarProductoCommand { Id = id });
            return Ok(resultado);
        }

        // POST /api/productos/{id}/movimientos
        [HttpPost("{id}/movements")]
        public async Task<IActionResult> RegistrarMovimiento(
            int id,
            [FromBody] CrearMovimientoStockCommand command)
        {
            command.ProductoId = id;
            var resultado = await _mediator.Send(command);
            return Ok(resultado);
        }

        // GET /api/productos/{id}/movimientos
        [HttpGet("{id}/movements")]
        public async Task<IActionResult> ObtenerMovimientos(int id)
        {
            var resultado = await _mediator.Send(new ObtenerMovimientosPorProductoQuery
            {
                ProductoId = id
            });
            return Ok(resultado);
        }
    }
}
