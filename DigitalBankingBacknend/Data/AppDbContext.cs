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
    }
}
