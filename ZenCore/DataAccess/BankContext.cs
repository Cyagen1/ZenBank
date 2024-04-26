using Microsoft.EntityFrameworkCore;
using ZenCore.Entities;

namespace ZenCore.DataAccess
{
    public class BankContext : DbContext
    {
        private readonly DataAccessSettings _settings;

        public BankContext(DbContextOptions options, DataAccessSettings settings) : base(options)
        {
            _settings = settings;
        }

        protected BankContext(DataAccessSettings settings)
        {
            _settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_settings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().Property(e => e.Amount).HasPrecision(11, 2);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
