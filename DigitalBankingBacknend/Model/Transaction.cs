namespace DigitalBankingBacknend.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; }

        //// Navigation properties
        //public Account FromAccount { get; set; } = null!;
        //public Account ToAccount { get; set; } = null!;
        //public FraudAlert? FraudAlert { get; set; }
    }
}
