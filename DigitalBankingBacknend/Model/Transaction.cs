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
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; }
    }
}
