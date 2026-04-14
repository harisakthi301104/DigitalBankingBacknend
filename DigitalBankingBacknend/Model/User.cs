using System.ComponentModel.DataAnnotations;

namespace DigitalBankingBacknend.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsMfaEnabled { get; set; } = false;
        public string? MfaSecretKey { get; set; }

        // One User → Many Accounts
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}

