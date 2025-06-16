using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clark_BankApp.Models.Entities;

namespace Clark_BankApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<CreditCard> CreditCard { get; set; }
        public DbSet<Customer> Customer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=info.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Accounts)
                .WithMany(a => a.Customers)
                .UsingEntity<CustomerAccount>()
                .HasKey("CustId", "AccountId");
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CreditCards)
                .WithMany(cc => cc.Customers)
                .UsingEntity<CustomerCreditCard>()
                .HasKey("CustId", "CardId");
            // modelBuilder.Entity<Customer>()
            //     .HasMany<Account>()
            //     .WithOne()
            //     .IsRequired()
            //     .UsingEntity<CustomerAccount>(
            //         l => l.HasOne<Customer>()
            //             .WithOne()
            //             .HasForeignKey(ca => ca.CustId),
            //         r => r.HasOne<Account>()
            //             .WithOne()
            //             .HasForeignKey(ca => ca.AccountId)
            //     );
            // modelBuilder.Entity<Customer>()
            //     .HasMany<CreditCard>()
            //     .WithOne()
            //     .IsRequired()
            //     .UsingEntity<CustomerCreditCard>(
            //         l => l.HasOne<Customer>()
            //             .WithOne()
            //             .HasForeignKey(cc => cc.CustId),
            //         r => r.HasOne<Account>()
            //             .WithOne()
            //             .HasForeignKey(cc => cc.CardId)
            //     );
        }
    }
}