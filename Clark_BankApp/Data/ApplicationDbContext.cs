using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_BankApp.Data
{
    public class ApplicationDbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<CreditCard> CreditCard { get; set; }
        public DbSet<Customer> Customer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=info.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Customer>()
                .HasOne<Account>()
                .WithOne()
                .IsRequired()
                .UsingEntity<CustomerAccount>(
                    l => l.HasOne<Customer>()
                        .WithOne()
                        .HasForeignKey(ca => ca.CustId),
                    r => r.HasOne<Account>()
                        .WithOne()
                        .HasForeignKey(ca => ca.AccountId)
                );
            modelBuilder.Entity<Customer>()
                .HasOne<CreditCard>()
                .WithOne()
                .IsRequired()
                .UsingEntity<CustomerCreditCard>(
                    l => l.HasOne<Customer>()
                        .WithOne()
                        .HasForeignKey(cc => cc.CustId),
                    r => r.HasOne<Account>()
                        .WithOne()
                        .HasForeignKey(cc => cc.CardId)
                );
        }
    }
}