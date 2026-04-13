using DigitalBankingBacknend.Data;
using DigitalBankingBacknend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBankingBacknend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudController : ControllerBase
    {
        private readonly FraudService _fraudService;
        private readonly AppDbContext _appDbContext;

        public FraudController(FraudService fraudService, AppDbContext appDbContext)
        {
            _fraudService = fraudService;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public ActionResult GetbyId(int id)
        {
            var alert = _appDbContext.FraudAlerts.FirstOrDefault(a => a.Id == id);
            if(alert == null) {
                return NotFound();
            }
            return Ok(alert);
        }
    }
}
