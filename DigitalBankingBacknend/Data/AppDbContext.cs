using DigitalBankingBacknend.Model;
using Microsoft.EntityFrameworkCore;

namespace DigitalBankingBacknend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<FraudAlert> FraudAlerts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User → Account (one to many)
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Account → Transaction as sender (one to many)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.FromAccount)
                .WithMany(a => a.SentTransactions)
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Account → Transaction as receiver (one to many)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ToAccount)
                .WithMany(a => a.ReceivedTransactions)
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction → FraudAlert (one to one)
            modelBuilder.Entity<FraudAlert>()
                .HasOne(f => f.Transaction)
                .WithOne(t => t.FraudAlert)
                .HasForeignKey<FraudAlert>(f => f.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    }
