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
            // 🔹 USER → ACCOUNT (One-to-Many)
            modelBuilder.Entity<Account>()
                .HasOne<User>()                      // Account belongs to User
                .WithMany()                          // User has many Accounts
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 ACCOUNT → TRANSACTION (Sender)
            modelBuilder.Entity<Transaction>()
                .HasOne<Account>()                   // From Account
                .WithMany()
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 ACCOUNT → TRANSACTION (Receiver)
            modelBuilder.Entity<Transaction>()
                .HasOne<Account>()                   // To Account
                .WithMany()
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 TRANSACTION → FRAUD ALERT (One-to-One OPTIONAL)
            modelBuilder.Entity<FraudAlert>()
                .HasOne<Transaction>()
                .WithOne()
                .HasForeignKey<FraudAlert>(f => f.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    }
