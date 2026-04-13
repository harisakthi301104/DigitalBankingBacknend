using System.ComponentModel.DataAnnotations;

namespace DigitalBankingBacknend.Model
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

    }
}
