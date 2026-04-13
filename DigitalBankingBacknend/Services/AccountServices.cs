using DigitalBankingBacknend.Data;
using DigitalBankingBacknend.Model;

namespace DigitalBankingBacknend.Services
{
    public class AccountServices
    {

        private readonly AppDbContext _context;

        public AccountServices(AppDbContext context)
        {
            _context = context;
        }

        public Account CreateAccount(int userId)
        {
            var account = new Account
            {
                UserId = userId,
                Balance = 1000 
            };

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account;
        }

        public Account GetAccount(int userId)
        {
            return _context.Accounts.FirstOrDefault(a => a.UserId == userId);
        }
    }
}
