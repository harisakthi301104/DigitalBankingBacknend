using DigitalBankingBacknend.Data;
using DigitalBankingBacknend.DTO;
using DigitalBankingBacknend.Hubs;
using DigitalBankingBacknend.Model;
using Microsoft.AspNetCore.SignalR;

namespace DigitalBankingBacknend.Services
{
    public class FraudService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hub;

        public FraudService(AppDbContext context, IHubContext<NotificationHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        // CHECK FRAUD — 5 simple rules
        public FraudcheckDto CheckFraud(TransferDTO dto)
        {
            int score = 0;
            var reasons = new List<string>();

            // Rule 1: Self transfer
            if (dto.FromAccountId == dto.ToAccountId)
            {
                score += 50;
                reasons.Add("Self transfer");
            }

            // Rule 2: High amount
            if (dto.Amount > 50000)
            {
                score += 50;
                reasons.Add("High amount");
            }

            // Rule 3: Too many transactions in last 1 minute
            var recentCount = _context.Transactions
                .Count(t => t.FromAccountId == dto.FromAccountId &&
                            t.Date > DateTime.UtcNow.AddMinutes(-1));
            if (recentCount > 3)
            {
                score += 40;
                reasons.Add("Too many transactions");
            }

            // Rule 4: Unusual hour (12am - 5am)
            var hour = DateTime.UtcNow.Hour;
            if (hour >= 0 && hour <= 5)
            {
                score += 30;
                reasons.Add("Unusual hour");
            }

            // Rule 5: Round number amount
            if (dto.Amount >= 10000 && dto.Amount % 10000 == 0)
            {
                score += 20;
                reasons.Add("Suspicious round amount");
            }

            return new FraudcheckDto
            {
                IsFraud = score >= 50,
                AccountId = dto.FromAccountId,
                Amount = dto.Amount,
                RiskScore = score,
                Reason = string.Join(", ", reasons)
            };
        }

        // SAVE ALERT + push real-time notification
        public async Task SaveAlert(FraudcheckDto dto)
        {
            // Save to DB
            var alert = new FraudAlert
            {
                TransactionId = dto.TransactionId,
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Reason = dto.Reason,
                RiskScore = dto.RiskScore,
                IsBlocked = dto.IsFraud,
                CreatedAt = DateTime.UtcNow
            };

            _context.FraudAlerts.Add(alert);
            await _context.SaveChangesAsync();

            // Push real-time alert to ALL connected clients
            await _hub.Clients.All.SendAsync("ReceiveFraudAlert", new
            {
                message = $"Transaction blocked: {dto.Reason}",
                amount = dto.Amount,
                riskScore = dto.RiskScore,
                time = DateTime.UtcNow.ToString("hh:mm:ss tt")
            });
        }

        // GET ALL ALERTS
        public List<FraudAlert> GetAllAlerts()
        {
            return _context.FraudAlerts
                .OrderByDescending(a => a.CreatedAt)
                .ToList();
        }

        // GET ALERT BY ID
        public FraudAlert? GetAlertById(int id)
        {
            return _context.FraudAlerts.FirstOrDefault(a => a.Id == id);
        }
    }
}
