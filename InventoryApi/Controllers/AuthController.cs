using Aplicacion.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
