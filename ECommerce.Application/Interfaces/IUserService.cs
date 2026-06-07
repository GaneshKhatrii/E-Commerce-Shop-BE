using ECommerce.Application.DTOs.User;

namespace ECommerce.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileResponseDto> GetUserProfileAsync(Guid userId);
        Task<string> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto request);
        Task<string> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request);
        Task<string> AddAddressAsync(Guid userId, AddAddressRequestDto request);
        Task<string> UpdateAddressAsync(Guid userId, Guid addressId, UpdateAddressRequestDto request);
        Task<List<AddressResponseDto>> GetUserAddressesAsync(Guid userId);
        Task<string> SetDefaultAddressAsync(Guid userId, Guid addressId);
        Task<string> DeleteAddressAsync(Guid userId, Guid addressId);
    }
}
