using System.ComponentModel.DataAnnotations;

namespace DigitalBankingBacknend.Model
{
    public class FraudAlert
    {
        [Key]
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation — back to Transaction
        public Transaction Transaction { get; set; } = null!;
    }
}

