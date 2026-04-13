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

    }
}
