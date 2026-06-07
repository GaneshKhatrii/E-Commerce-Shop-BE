using ECommerce.API.Helpers;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs.User;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // using lambda operator
        private Guid userId => CurrentUserHelper.GetUserId(User);

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _userService.GetUserProfileAsync(userId);

            return Ok(new ApiResponse<UserProfileResponseDto>()
            {
                Success = true,
                StatusCode = 200,
                Message = "Profile fetched successfully",
                Data = result
            });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequestDto request)
        {
            var result = await _userService.UpdateProfileAsync(userId, request);

            return Ok(new ApiResponse<string>()
            {
                Success = true,
                StatusCode = 200,
                Message = result,
                Data = null
            });
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto request)
        {
            var result = await _userService.ChangePasswordAsync(userId, request);

            return Ok(new ApiResponse<string>()
            {
                Success = true,
                StatusCode = 200,
                Message = result,
                Data = null
            });
        }

        [HttpPost("addresses")]
        public async Task<IActionResult> AddAddress(AddAddressRequestDto request)
        {
            var result = await _userService.AddAddressAsync(userId, request);

            return Ok(new ApiResponse<string>()
            {
                Success = true,
                StatusCode = 201,
                Message = result,
                Data = null
            });
        }

        [HttpPut("addresses/{addressId}")]
        public async Task<IActionResult> UpdateAddress(Guid addressId, [FromBody] UpdateAddressRequestDto request)
        {
            var result = await _userService.UpdateAddressAsync(userId, addressId, request);
            return Ok(new ApiResponse<string>()
            {
                Success = true,
                StatusCode = 200,
                Message = result,
                Data = null
            });
        }

        [HttpGet("addresses")]
        public async Task<IActionResult> GetAddresses()
        {
            var result = await _userService.GetUserAddressesAsync(userId);

            return Ok(new ApiResponse<List<AddressResponseDto>>()
            {
                Success = true,
                StatusCode = 200,
                Message = "Addresses fetched successfully",
                Data = result
            });
        }

        [HttpPut("addresses/{addressId}/default")]
        public async Task<IActionResult> SetDefaultAddress(Guid addressId)
        {
            var result = await _userService.SetDefaultAddressAsync(userId, addressId);

            return Ok(new ApiResponse<string>()
            {
                Success = true,
                StatusCode = 200,
                Message = result,
                Data = null
            });
        }

        [HttpDelete("addresses/{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var result = await _userService.DeleteAddressAsync(userId, addressId);
            return Ok(new ApiResponse<string>()
            {
                Success = true,
                StatusCode = 200,
                Message = result,
            });
        }
    }
}
