using ECommerce.Application.DTOs.User;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileResponseDto> GetUserProfileAsync(Guid userId)
        {
            // Fetch user with user id
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return new UserProfileResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailVerified = user.IsEmailVerified,
            };
        }

        public async Task<string> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Update properties
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync();
            return "Profile updated successfully";
        }

        public async Task<string> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash);

            if (!isCurrentPasswordValid)
            {
                throw new Exception("Current password is incorrect");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.SaveChangesAsync();
            return "Password changed successfully";
        }

        public async Task<string> AddAddressAsync(Guid userId, AddAddressRequestDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (request.IsDefault)
            {
                var addresses = await _userRepository
                    .GetUserAddressesAsync(userId);

                foreach (var existingAddress in addresses)
                {
                    existingAddress.IsDefault = false;
                }
            }

            var address = new Address
            {
                UserId = userId,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                Country = request.Country,
                PostalCode = request.PostalCode,
                AddressType = request.AddressType,
                IsDefault = request.IsDefault,
            };

            await _userRepository.AddAddressAsync(address);
            await _userRepository.SaveChangesAsync();
            return "Address added successfully";
        }

        public async Task<string> UpdateAddressAsync(Guid userId, Guid addressId, UpdateAddressRequestDto request)
        {
            var address = await _userRepository.GetAddressByIdAsync(userId, addressId);

            if (address == null)
            {
                throw new Exception("Address not found");
            }

            address.FullName = request.FullName;
            address.PhoneNumber = request.PhoneNumber;
            address.AddressLine1 = request.AddressLine1;
            address.AddressLine2 = request.AddressLine2;
            address.City = request.City;
            address.State = request.State;
            address.Country = request.Country;
            address.PostalCode = request.PostalCode;
            address.AddressType = request.AddressType;
            address.UpdatedAt = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync();
            return "Address updated successfully";
        }

        public async Task<List<AddressResponseDto>> GetUserAddressesAsync(Guid userId)
        {
            var addresses = await _userRepository.GetUserAddressesAsync(userId);

            return addresses.Select(address => new AddressResponseDto
            {
                Id = address.Id,
                FullName = address.FullName,
                PhoneNumber = address.PhoneNumber,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode,
                AddressType = address.AddressType,
                IsDefault = address.IsDefault,
            }).ToList();
        }

        public async Task<string> SetDefaultAddressAsync(Guid userId, Guid addressId)
        {
            var addresses = await _userRepository.GetUserAddressesAsync(userId);

            // First reset all address IsDefault value to false
            foreach (var address in addresses)
            {
                address.IsDefault = false;
            }

            var selectedAddress = addresses.FirstOrDefault(x => x.Id == addressId);

            if (selectedAddress == null)
            {
                throw new Exception("Address not found");
            }

            selectedAddress.IsDefault = true;
            await _userRepository.SaveChangesAsync();
            return "Default address successfully";
        }

        public async Task<string> DeleteAddressAsync(Guid userId, Guid addressId)
        {
            var address = await _userRepository.GetAddressByIdAsync(userId, addressId);

            if (address == null)
            {
                throw new Exception("Address not found");
            }

            _userRepository.DeleteAddressAsync(address);

            await _userRepository.SaveChangesAsync();

            return "Address deleted successfully";
        }
    }
}
