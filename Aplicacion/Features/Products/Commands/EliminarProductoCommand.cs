using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Aplicacion.Wrappers;
using MediatR;

namespace Aplicacion.Features.Products.Commands
{
    public class EliminarProductoCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }

        public class EliminarProductoHandler : IRequestHandler<EliminarProductoCommand, Response<bool>>
        {
            private readonly IProductoRepositoryAsync _repositorio;

            public EliminarProductoHandler(IProductoRepositoryAsync repositorio)
            {
                _repositorio = repositorio;
            }

            public async Task<Response<bool>> Handle(EliminarProductoCommand request, CancellationToken cancellationToken)
            {
                var producto = await _repositorio.ObtenerPorIdAsync(request.Id)
                    ?? throw new ApiException($"Producto con Id {request.Id} no encontrado.");

                await _repositorio.EliminarAsync(producto);

                return new Response<bool>(true);
            }
        }
    }
}
