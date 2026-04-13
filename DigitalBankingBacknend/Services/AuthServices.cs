using DigitalBankingBacknend.Data;
using DigitalBankingBacknend.DTO;
using DigitalBankingBacknend.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigitalBankingBacknend.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }
        public string Register(RegisterDTO dto)
        {
            var userExists = _context.Users.Any(u => u.Email == dto.Email);

            if (userExists)
                return "User already exists";

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = ""
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return "Registered successfully";
        }
        public object Login(LoginDTO dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email && u.PasswordHash == dto.Password);

            if (user == null)
                return null;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
                return null;

            var token = GenerateJwtToken(user);
            return token;
        }

        // 🔐 TOKEN GENERATION
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("super_secret_key_12345"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Name),
            new Claim(ClaimTypes.Name, user.Email)
        };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
