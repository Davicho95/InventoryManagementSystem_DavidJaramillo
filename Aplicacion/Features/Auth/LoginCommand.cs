using Aplicacion.Dto.Auth;
using Aplicacion.Interfaces.Auth;
using Aplicacion.Wrappers;
using MediatR;

namespace Aplicacion.Features.Auth
{
    public class LoginCommand : IRequest<Response<AuthResponseDto>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public class LoginHandler : IRequestHandler<LoginCommand, Response<AuthResponseDto>>
        {
            private readonly IAuthService _authService;

            public LoginHandler(IAuthService authService)
            {
                _authService = authService;
            }

            public async Task<Response<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var dto = new LoginDto
                {
                    Email = request.Email,
                    Password = request.Password
                };

                var result = await _authService.LoginAsync(dto);
                return new Response<AuthResponseDto>(result);
            }
        }
    }
}
