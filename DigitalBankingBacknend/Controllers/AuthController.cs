using DigitalBankingBacknend.DTO;
using DigitalBankingBacknend.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBankingBacknend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // ✅ REGISTER
        [HttpPost("register")]
        public IActionResult Register(RegisterDTO dto)
        {
            return Ok(_authService.Register(dto));
        }

        // ✅ LOGIN
        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var result = _authService.Login(dto);

            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        // 🔐 MFA SETUP
        [HttpPost("mfa/setup")]
        public IActionResult SetupMfa(string email)
        {
            var result = _authService.SetupMfa(email);

            if (result == "User not found")
                return NotFound(result);

            return Ok(new { secretKey = result });
        }

        // 🔐 MFA CONFIRM
        [HttpPost("mfa/confirm")]
        public IActionResult ConfirmMfa(MfaDto dto)
        {
            var result = _authService.ConfirmMfa(dto.Email, dto.Code);

            if (!result)
                return BadRequest("Invalid OTP");

            return Ok("MFA Enabled");
        }

        // 🔐 MFA LOGIN
        [HttpPost("mfa/login")]
        public IActionResult VerifyLoginOtp(MfaDto dto)
        {
            var token = _authService.VerifyLoginOtp(dto.Email, dto.Code);

            if (token == null)
                return Unauthorized("Invalid OTP");

            return Ok(token);
        }
    }
}
