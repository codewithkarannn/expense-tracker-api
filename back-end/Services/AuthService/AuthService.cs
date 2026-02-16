using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Budget_Tracker_WebAPI.Repositories;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Budget_Tracker_WebAPI.Services
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
    {
        public UserDetailModel? GetUserDetail(Guid userId)
        {
            try
            {
                return userRepository.GetUserDetail(userId);
            }
            catch (Exception ex)
            {

                throw new Exception("There is the problem  getting the user details. Please try agian later", ex);
            }
        }

        public async Task ResgisterUserAsync(RegisterUserDTO registerUserDto)
        {
            try
            {
                if (await userRepository.IsEmailExistsAsync(registerUserDto.Email))
                {

                    throw new ArgumentException("Email already exists. Please use a different email.");

                }
             

                var passwordHash = HashPassword(registerUserDto.Password);

                var user = new UserMaster
                {

                    UserPassword = passwordHash,
                    UserEmail = registerUserDto.Email,
                    FirstName =  registerUserDto.FirstName,
                    LastName = registerUserDto.LastName,
                    CurrencyMasterId =  registerUserDto.CurrencyMasterId,
                    UserRoleId =   2
                };

                await userRepository.AddUserAsync(user);
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

                throw new Exception("There is the problem in signing up. Please try agian later", ex);
            }
        }

        public async Task<string> LoginUserAsync(LoginUserDTO  loginUserDto)
        {
            // Validate user credentials
            var user = await userRepository.GetUserByEmailAsync(loginUserDto.UserEmail);

            if (user == null || !VerifyPassword(loginUserDto.Password, user.UserPassword))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            // Generate JWT Token
            var token = GenerateJwtToken(user);

            return token;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password))) == storedHash;
            }
        }
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        private string GenerateJwtToken(UserMaster user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserMasterId.ToString()),
            new Claim(ClaimTypes.Email, user.UserEmail)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                               signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
