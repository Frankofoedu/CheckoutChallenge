using Microsoft.EntityFrameworkCore;
using PaymentGateway.Core.Entities;

namespace PaymentGateway.Core.Data
{
    /// <Note>
    /// DbSet properties are being used by generic repository
    /// </Note>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}