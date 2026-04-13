namespace DigitalBankingBacknend.Model
{
    public class FraudAlert
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string Reason { get; set; }
        public int RiskScore { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

