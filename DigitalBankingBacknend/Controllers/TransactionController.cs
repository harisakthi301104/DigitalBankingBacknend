using DigitalBankingBacknend.DTO;
using DigitalBankingBacknend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBankingBacknend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _service;

        public TransactionController(TransactionService service)
        {
            _service = service;
        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferDTO dto)
        {
            var result = _service.Transfer(dto);
            return Ok(result);
        }
    }
}
