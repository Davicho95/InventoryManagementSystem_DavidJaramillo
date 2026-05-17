using Aplicacion.Dto.Auth;

namespace Aplicacion.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
