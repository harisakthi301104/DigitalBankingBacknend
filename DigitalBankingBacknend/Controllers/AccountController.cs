using DigitalBankingBacknend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBankingBacknend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly AccountServices _service;

        public AccountController(AccountServices service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult Create(int userId)
        {
            return Ok(_service.CreateAccount(userId));
        }

        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            return Ok(_service.GetAccount(userId));
        }
    }
}
