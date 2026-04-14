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

        // ✅ 1. Transfer Money
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDTO dto)
        {
            if (dto.Amount <= 0)
                return BadRequest("Amount must be greater than zero");

            var result = await _service.Transfer(dto);

            return Ok(result);
        }

        // ✅ 2. Get Transaction By ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var txn = _service.GetById(id);

            if (txn == null)
                return NotFound("Transaction not found");

            return Ok(txn);
        }

        // ✅ 3. Transaction History with Pagination
        [HttpGet("history/{accountId}")]
        public IActionResult GetHistory(int accountId, int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Invalid pagination values");

            var data = _service.GetHistory(accountId, page, pageSize);

            return Ok(data);
        }

        // ✅ 4. Filter by Date Range
        [HttpGet("filter")]
        public IActionResult FilterByDate(
            int accountId,
            DateTime fromDate,
            DateTime toDate)
        {
            if (fromDate > toDate)
                return BadRequest("Invalid date range");

            var data = _service.FilterByDate(accountId, fromDate, toDate);

            return Ok(data);
        }

        // ✅ 5. Dashboard Summary
        [HttpGet("summary/{accountId}")]
        public IActionResult GetSummary(int accountId)
        {
            var summary = _service.GetSummary(accountId);

            return Ok(summary);
        }
    }
}
