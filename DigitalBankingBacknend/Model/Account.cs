namespace DigitalBankingBacknend.Model
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        //// Navigation properties
        //public User User { get; set; } = null!;
        //public ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();
        //public ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
    }
}
