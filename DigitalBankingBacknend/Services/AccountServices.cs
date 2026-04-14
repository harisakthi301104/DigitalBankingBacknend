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
    
        public Account CreateAccount(int userId, string accountType)
        {
            var account = new Account
            {
                UserId = userId,
                AccountType = accountType,
                AccountNumber = GenerateAccountNumber(),
                Balance = 0,
                Status = "Active"
            };

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account;
        }

        private string GenerateAccountNumber()
        {
            var random = new Random();
            return "NB" + random.Next(100000000, 999999999).ToString();
        }


        public Account GetAccountById(int accountId)
        {
            return _context.Accounts.FirstOrDefault(a => a.Id == accountId);
        }

    
        public List<Account> GetAllByUser(int userId)
        {
            return _context.Accounts
                .Where(a => a.UserId == userId)
                .ToList();
        }

        public string Deposit(int accountId, decimal amount)
        {

            if (amount <= 0)
                return "Invalid amount";

            var account = _context.Accounts.Find(accountId);

            if (account == null)
                return "Account not found";

            account.Balance += amount;

            var txn = new Transaction
            {
                FromAccountId = accountId,
                Amount = amount,
                TransactionType = "Deposit",
                Status = "Success"
            };

            _context.Transactions.Add(txn);
            _context.SaveChanges();

            return $"Deposit successful. New Balance: {account.Balance}";
        }

    
        public string Withdraw(int accountId, decimal amount)
        {
            if (amount <= 0)
                return "Invalid amount";

            var account = _context.Accounts.Find(accountId);

            if (account == null)
                return "Account not found";

            if (account.Balance < amount)
                return "Insufficient balance";

            account.Balance -= amount;

            var txn = new Transaction
            {
                FromAccountId = accountId,
                Amount = amount,
                TransactionType = "Withdraw",
                Status = "Success"
            };

            _context.Transactions.Add(txn);
            _context.SaveChanges();

            return $"Withdrawal successful. New Balance: {account.Balance}";
        }
        public string FreezeAccount(int accountId)
        {
            var account = _context.Accounts.Find(accountId);

            if (account == null)
                return "Account not found";

            if (account.Status == "Frozen")
                return "Account already frozen";

            account.Status = "Frozen";
            _context.SaveChanges();

            return "Account has been frozen successfully";
        }
        public string UnfreezeAccount(int accountId)
        {
            var account = _context.Accounts.Find(accountId);

            if (account == null)
                return "Account not found";

            if (account.Status == "Active")
                return "Account is already active";

            account.Status = "Active";
            _context.SaveChanges();

            return "Account has been reactivated";
        }
    }
}
