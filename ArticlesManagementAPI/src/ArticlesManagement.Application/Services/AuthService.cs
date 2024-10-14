using ArticlesManagement.Application.Interfaces;
using ArticlesManagement.Application.Models.Requests;
using ArticlesManagement.Application.Models.Responses;
using ArticlesManagement.Application.Models.Results;
using ArticlesManagement.Domain.Abstractions;
using ArticlesManagement.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArticlesManagement.Application.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmail(request.Email);
            if (existingUser != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Errors = new[] { "User already exists." }
                };
            }

            var userModel = new User 
            {
                Email = request.Email,
                PasswordHash = HashPassword(request.Password)
            };

            var user = await _userRepository.AddUser(userModel);

            var verificationCode = await GenerateVerificationCode(user.Id);

            return new RegisterResponse
            {
                Success = true,
                VerificationCode = verificationCode,
            };
        }



        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByEmail(request.Email);
            if (user == null || !VerifyPassword(user.PasswordHash, request.Password)) // Implement password verification
            {
                return new LoginResponse
                {
                    Errors = new[] { "Invalid email or password." }
                };
            }

            return new LoginResponse
            {
                Token = GenerateJwtToken(user)
            };
        }

        public async Task<string> RegenerateVerificationCode(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            return await GenerateVerificationCode(user.Id);

        }

        public async Task<bool> VerifyEmail(VerifyEmailRequest request)
        {
            var user = await _userRepository.GetByEmail(request.Email);

            return await _userRepository.VerifyUser(user.Id, request.VerificationCode);
        }

        public async Task<GetUserResponse> GetUserById(int id)
        {
            var user = await _userRepository.GetById(id);

            var response = _mapper.Map<GetUserResponse>(user);

            return response;
        }

        private async Task<string> GenerateVerificationCode(int userId)
        {
            Random random = new Random();
            var verificationCode = random.Next(10000000, 99999999).ToString(); // Generate a number 8 digit number

            await _userRepository.StoreVerificationCode(userId, verificationCode);

            return verificationCode;

        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

            // Create a security key from the key stored in configuration
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:Key"]));

            // Create signing credentials using the key and the HMAC SHA256 algorithm
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Jwt:Issuer"], // Issuer of the token
                audience: _configuration["Authentication:Jwt:Audience"], // Audience for the token
                claims: claims, // Claims to include in the token
                expires: DateTime.Now.AddDays(7), // Set token expiration time
                signingCredentials: creds // Signing credentials
            );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string storedHash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

    }
}
