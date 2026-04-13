using DigitalBankingBacknend.Data;
using DigitalBankingBacknend.Model;

namespace DigitalBankingBacknend.Services
{
    public class TransactionService
    {
        private readonly AppDbContext _context;
        //private readonly FraudService _fraudService;

        public TransactionService(AppDbContext context)
        {
            _context = context;
            //_fraudService = fraudService;
        }

        public string Transfer(int fromId, int toId, decimal amount)
        {
            var from = _context.Accounts.Find(fromId);
            var to = _context.Accounts.Find(toId);

            if (from.Balance < amount)
                return "Insufficient Balance";

            // Fraud Check
            //var isFraud = _fraudService.CheckFraud(fromId, amount);

            //if (isFraud)
            //    return "Transaction Blocked (Fraud Detected)";

            from.Balance -= amount;
            to.Balance += amount;

            var txn = new Transaction
            {
                FromAccountId = fromId,
                ToAccountId = toId,
                Amount = amount,
                Status = "Success"

            };

            _context.Transactions.Add(txn);
            _context.SaveChanges();

            return "Transaction Successful";
        }
    }
}
