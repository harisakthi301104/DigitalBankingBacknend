using DigitalBankingBacknend.Data;
using DigitalBankingBacknend.DTO;
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

        public string Transfer(TransferDTO dto)
        {
            var from = _context.Accounts.Find(dto.FromAccountId);
            var to = _context.Accounts.Find(dto.ToAccountId);

            if (from == null || to == null)
                return "Invalid Accounts";

            if (from.Balance < dto.Amount)
                return "Insufficient Balance";

            //var isFraud = _fraudService.CheckFraud(dto.FromAccountId, dto.Amount);

            //if (isFraud)
            //    return "Transaction Blocked (Fraud Detected)";

            from.Balance -= dto.Amount;
            to.Balance += dto.Amount;

            var txn = new Transaction
            {
                FromAccountId = dto.FromAccountId,
                ToAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                Status = "Success"
            };

            _context.Transactions.Add(txn);
            _context.SaveChanges();

            return "Transaction Successful";
        }
    }
}