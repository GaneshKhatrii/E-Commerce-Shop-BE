using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Auth;

namespace ECommerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> VerifyEmailAsync(string token);
        Task<ApiResponse<string>> RegisterAsync(RegisterRequestDto request);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    }
}
