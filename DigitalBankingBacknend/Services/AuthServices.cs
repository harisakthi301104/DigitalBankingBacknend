using DigitalBankingBacknend.Data;
using DigitalBankingBacknend.DTO;
using DigitalBankingBacknend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
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

        // ✅ REGISTER
        public string Register(RegisterDTO dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return "User already exists";

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return "Registered successfully";
        }

        // ✅ LOGIN
        public object Login(LoginDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
                return "Invalid credentials";

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                return "Invalid credentials";

            // 🔐 MFA CHECK
            if (user.IsMfaEnabled)
                return "MFA_REQUIRED";

            return GenerateJwtToken(user);
        }

        // 🔐 MFA SETUP (returns secret key)
        public string SetupMfa(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return "User not found";

            var secretBytes = KeyGeneration.GenerateRandomKey(20);
            var secretKey = Base32Encoding.ToString(secretBytes);

            user.MfaSecretKey = secretKey;
            _context.SaveChanges();

            return secretKey; // user adds this to Google Authenticator
        }

        // 🔐 MFA CONFIRM (enable MFA)
        public bool ConfirmMfa(string email, string code)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || user.MfaSecretKey == null)
                return false;

            var totp = new Totp(Base32Encoding.ToBytes(user.MfaSecretKey));

            if (!totp.VerifyTotp(code, out _))
                return false;

            user.IsMfaEnabled = true;
            _context.SaveChanges();

            return true;
        }

        // 🔐 VERIFY OTP DURING LOGIN
        public string VerifyLoginOtp(string email, string code)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || user.MfaSecretKey == null)
                return null;

            var totp = new Totp(Base32Encoding.ToBytes(user.MfaSecretKey));

            if (!totp.VerifyTotp(code, out _))
                return null;

            return GenerateJwtToken(user);
        }

        // 🔐 JWT TOKEN GENERATION
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
