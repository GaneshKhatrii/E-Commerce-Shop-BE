using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.Extensions.Configuration;
using ECommerce.Domain.Enums;
using ECommerce.Application.Common;

namespace ECommerce.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<ApiResponse<string>> VerifyEmailAsync(string token)
        {
            var user = await _userRepository.GetUserByVerificationTokenAsync(token);

            if (user == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Invalid verification token"
                };
            }

            if (user.EmailVerificationTokenExpiry < DateTime.UtcNow)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Verification token expired"
                };
            }

            user.IsEmailVerified = true;

            user.EmailVerificationToken = null;

            user.EmailVerificationTokenExpiry = null;

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 200,
                Message = "Email verified successfully"
            };
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequestDto request)
        {
            // Check if email already exists
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Email already exists"
                };
            }

            // Generate verification token
            var verificationToken = Guid.NewGuid().ToString();

            // Create new user
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,

                // Hash password
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),

                Role = UserRole.User,

                IsEmailVerified = false,

                EmailVerificationToken = verificationToken,

                EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };

            // Save user
            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            // Verification link

            // Through Backend
            //var verificationLink = $"{_configuration["AppSettings:BaseUrl"]}/api/auth/verify-email?token={verificationToken}";    

            // Through Frontend
            var verificationLink = $"{_configuration["AppSettings:FrontBaseUrl"]}/verify-email?token={verificationToken}";


            // Send verification email
            var emailBody = $@"
                <h2>Email Verification</h2>
                <p>Please click below link to verify your email:</p>
                <a href='{verificationLink}'>Verify Email</a>
            ";

            await _emailService.SendEmailAsync(
                user.Email,
                "Verify Your Email",
                emailBody
            );

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 201,
                Message = "Registration successful. Please verify your email.",
            };
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            // Find user
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                return new ApiResponse<LoginResponseDto>
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Invalid email or password",
                };
            }
            // Verify password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return new ApiResponse<LoginResponseDto>
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Invalid email or password",
                };
            }

            // Check email verification
            if (!user.IsEmailVerified)
            {
                return new ApiResponse<LoginResponseDto>
                {
                    Success = false,
                    StatusCode = 403,
                    Message = "Please verify your email first",
                };
            }

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);

            return new ApiResponse<LoginResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Login successful",
                Data = new LoginResponseDto
                {
                    Token = token,
                    Email = user.Email,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Role = user.Role.ToString()
                }
            };
        }
    }
}
