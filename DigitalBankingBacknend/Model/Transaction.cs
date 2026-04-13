using System.ComponentModel.DataAnnotations;

namespace DigitalBankingBacknend.Model
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; } // Transfer / Deposit / Withdrawal
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }

        public bool IsFlagged { get; set; } = false;

        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; }
    }
}