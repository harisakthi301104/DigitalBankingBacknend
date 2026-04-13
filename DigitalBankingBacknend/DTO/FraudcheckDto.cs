namespace DigitalBankingBacknend.DTO
{
    public class FraudcheckDto
    {
        public bool IsFraud { get; set; }
        public int TransactionId { get; set; }
        public int RiskScore { get; set; }
        public string Reason { get; set; }


    }
}
