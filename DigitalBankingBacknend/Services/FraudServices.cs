using DigitalBankingBacknend.Data;
using DigitalBankingBacknend.DTO;
using DigitalBankingBacknend.Model;

namespace DigitalBankingBacknend.Services
{
    public class FraudService
    {
        private readonly AppDbContext _context;

        public FraudService(AppDbContext context)
        {
            _context = context;
        }

        public FraudcheckDto CheckFraud(TransferDTO dto)
        {
            int score = 0;
            string reason = "";

            // Rule 1: High amount
            if (dto.Amount > 50000)
            {
                score += 50;
                reason = "High amount";
            }

            // Rule 2: Too many transactions (last 1 min)
            var count = _context.Transactions
                .Count(t => t.FromAccountId == dto.FromAccountId &&
                            t.Date > DateTime.Now.AddMinutes(-1));

            if (count > 3)
            {
                score += 30;
                reason = "Too many transactions";
            }

            // Rule 3: Same account transfer
            if (dto.FromAccountId == dto.ToAccountId)
            {
                score += 50;
                reason = "Invalid self transfer";
            }

            // Final decision (simple)
            bool isFraud = score >= 50;

            return new FraudcheckDto
            {
                IsFraud = isFraud,
                RiskScore = score,
                Reason = reason
            };
        }

        public async Task SaveAlert(FraudcheckDto dto)
        {
            var alert = new FraudAlert
            {
                TransactionId = dto.TransactionId,
                Reason = dto.Reason,
                RiskScore = dto.RiskScore,
                CreatedAt = DateTime.Now
            };

            _context.FraudAlerts.Add(alert);
            await _context.SaveChangesAsync();
        }
    }
}
