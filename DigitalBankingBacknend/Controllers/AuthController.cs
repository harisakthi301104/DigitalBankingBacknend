using DigitalBankingBacknend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBankingBacknend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] DTO.RegisterDTO dto)
        {
            var result = _authService.Register(dto);
            if (result == "User already exists")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] DTO.LoginDTO dto)
        {
            var result = _authService.Login(dto);
            if (result == null)
                return Unauthorized("Invalid credentials");
            return Ok(result);
        }
    }
}
