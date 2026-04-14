using DigitalBankingBacknend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBankingBacknend.Controllers
{
    [Route("api/fraud")]
    [ApiController]
    [Authorize]
    public class FraudController : ControllerBase
    {
        private readonly FraudService _fraudService;

        public FraudController(FraudService fraudService)
        {
            _fraudService = fraudService;
        }

        // GET /api/fraud/alerts
        [HttpGet("alerts")]
        public IActionResult GetAllAlerts()
        {
            return Ok(_fraudService.GetAllAlerts());
        }


        // GET /api/fraud/alerts/{id}
        [HttpGet("alerts/{id}")]
        public IActionResult GetAlertById(int id)
        {
            var alert = _fraudService.GetAlertById(id);
            if (alert == null)
                return NotFound(new { message = "Alert not found." });
            return Ok(alert);
        }
    }
}
