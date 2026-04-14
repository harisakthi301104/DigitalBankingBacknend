namespace DigitalBankingBacknend.DTO
{
    public class TransferHistoryDTO
    {
        public string ReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
