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
        public IActionResult Create(int userId, string accountType)
        {
            var account = _service.CreateAccount(userId, accountType);
            return Ok(account);
        }

    
        [HttpGet("{accountId}")]
        public IActionResult GetById(int accountId)
        {
            var account = _service.GetAccountById(accountId);

            if (account == null)
                return NotFound("Account not found");

            return Ok(account);
        }

    
        [HttpGet("user/{userId}")]
        public IActionResult GetByUser(int userId)
        {
            var accounts = _service.GetAllByUser(userId);
            return Ok(accounts);
        }

        [HttpPost("deposit")]
        public IActionResult Deposit(int accountId, decimal amount)
        {
            return Ok(_service.Deposit(accountId, amount));
        }

    
        [HttpPost("withdraw")]
        public IActionResult Withdraw(int accountId, decimal amount)
        {
            return Ok(_service.Withdraw(accountId, amount));
        }
        [HttpPost("freeze/{accountId}")]
        public IActionResult Freeze(int accountId)
        {
            return Ok(_service.FreezeAccount(accountId));
        }

        [HttpPost("unfreeze/{accountId}")]
        public IActionResult Unfreeze(int accountId)
        {
            return Ok(_service.UnfreezeAccount(accountId));
        }
    }
}
