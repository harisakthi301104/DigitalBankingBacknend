using DigitalBankingBacknend.Data;
using DigitalBankingBacknend.DTO;
using DigitalBankingBacknend.Model;

namespace DigitalBankingBacknend.Services
{
    public class TransactionService
    {
        private readonly AppDbContext _context;
        private readonly FraudService _fraudService;

        public TransactionService(AppDbContext context, FraudService fraudService)
        {
            _context = context;
            _fraudService = fraudService;
        }

        public async Task<string> Transfer(TransferDTO dto)
        {
            var from = await _context.Accounts.FindAsync(dto.FromAccountId);
            var to = await _context.Accounts.FindAsync(dto.ToAccountId);

            // Step 1: Validate
            if (from == null || to == null)
                return "Invalid accounts";

            // Step 2: Status check
            if (from.Status != "Active" || to.Status != "Active")
                return "One of the accounts is frozen";

            // Step 3: Balance check
            if (from.Balance < dto.Amount)
                return "Insufficient balance";

            // Step 4: Fraud check
            var fraudResult = _fraudService.CheckFraud(new TransferDTO
            {
                FromAccountId = dto.FromAccountId,
                ToAccountId = dto.ToAccountId,
                Amount = dto.Amount
            });

            if (fraudResult.IsFraud)
            {
                await _fraudService.SaveAlert(fraudResult);
                return $"Blocked: {fraudResult.Reason}";
            }

            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Step 6: Debit
                from.Balance -= dto.Amount;

                // Step 7: Credit
                to.Balance += dto.Amount;

                // Step 8: Save transaction
                var txn = new Transaction
                {
                    FromAccountId = dto.FromAccountId,
                    ToAccountId = dto.ToAccountId,
                    Amount = dto.Amount,
                    TransactionType = "Transfer",
                    ReferenceNumber = GenerateReference(),
                    Description = dto.Description,
                    Status = "Success"
                };

                _context.Transactions.Add(txn);

                await _context.SaveChangesAsync();

                // Step 9: Commit
                await dbTransaction.CommitAsync();

                return "Transfer Successful";
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                return "Transaction Failed";
            }
        }
        private string GenerateReference()
        {
            return "TXN" + DateTime.Now.Ticks;
        }
        public Transaction GetById(int id)
        {
            return _context.Transactions.FirstOrDefault(t => t.Id == id);
        }
        public List<TransferHistoryDTO> GetHistory(int accountId, int page, int pageSize)
        {
            return _context.Transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
                .OrderByDescending(t => t.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransferHistoryDTO
                {
                    ReferenceNumber = t.ReferenceNumber,
                    Amount = t.Amount,
                    TransactionType = t.TransactionType,
                    Date = t.Date,
                    Status = t.Status
                })
                .ToList();
        }
        public List<TransferHistoryDTO> FilterByDate(int accountId, DateTime from, DateTime to)
        {
            return _context.Transactions
                .Where(t => (t.FromAccountId == accountId || t.ToAccountId == accountId)
                         && t.Date >= from && t.Date <= to)
                .Select(t => new TransferHistoryDTO
                {
                    ReferenceNumber = t.ReferenceNumber,
                    Amount = t.Amount,
                    TransactionType = t.TransactionType,
                    Date = t.Date,
                    Status = t.Status
                })
                .ToList();
        }
        public TransferSummaryDTO GetSummary(int accountId)
        {
            var txns = _context.Transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId);

            return new TransferSummaryDTO
            {
                TotalCredits = txns.Where(t => t.ToAccountId == accountId).Sum(t => t.Amount),
                TotalDebits = txns.Where(t => t.FromAccountId == accountId).Sum(t => t.Amount),
                TotalTransactions = txns.Count()
            };
        }

    }
}