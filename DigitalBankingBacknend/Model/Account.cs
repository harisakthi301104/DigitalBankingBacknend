using System.ComponentModel.DataAnnotations;

namespace DigitalBankingBacknend.Model
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string AccountNumber { get; set; }  

        public string AccountType { get; set; }   

        public string Status { get; set; } = "Active";

        public decimal Balance { get; set; }

        //// Navigation properties
        //public User User { get; set; } = null!;
        //public ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();
        //public ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
    }
}
