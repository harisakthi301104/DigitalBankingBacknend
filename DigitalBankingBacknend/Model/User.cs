using System.ComponentModel.DataAnnotations;

namespace DigitalBankingBacknend.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";

        public string? MfaSecretKey { get; set; }   // Base32 key
        public bool IsMfaEnabled { get; set; } = false;

    }
}

