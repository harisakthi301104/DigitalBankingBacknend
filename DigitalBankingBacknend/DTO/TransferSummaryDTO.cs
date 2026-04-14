namespace DigitalBankingBacknend.DTO
{
    public class TransferSummaryDTO
    {
        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
        public int TotalTransactions { get; set; }
    }
}
