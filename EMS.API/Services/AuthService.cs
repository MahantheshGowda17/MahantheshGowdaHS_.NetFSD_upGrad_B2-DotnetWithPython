using EMS.API.Data;
using EMS.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static EMS.API.DTOs.EmployeeDtos;
using Microsoft.EntityFrameworkCore;

namespace EMS.API.Services
{
    public class AuthService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // REGISTER
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Username already exists"
                };
            }

            var user = new AppUser
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role ?? "Viewer",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                Username = user.Username,
                Role = user.Role,
                Token = GenerateToken(user),
                Message = "Registered successfully"
            };
        }

        // LOGIN
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            return new AuthResponseDto
            {
                Success = true,
                Username = user.Username,
                Role = user.Role,
                Token = GenerateToken(user),
                Message = "Login successful"
            };
        }

        // GENERATE JWT TOKEN
        private string GenerateToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(
                     double.Parse(_config["Jwt:ExpiryHours"] ?? "8")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
