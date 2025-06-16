using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clark_BankApp.Data;
using Clark_BankApp.Interfaces;
using Clark_BankApp.Models.Dtos;
using Clark_BankApp.Models.Entities;

namespace Clark_BankApp.Services
{
    public class BankQueryService : IBankQuery
    {
        private readonly ApplicationDbContext _context;

        public BankQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1
        /// Returns a summary of all customers, including their name, credit score, 
        public async Task<List<CustomerSummaryDto>> CustomerSummariesAsync()
        {
            return await _context.Customer
                .Include(customer => customer.Accounts)
                .Include(customer => customer.CreditCards)
                .Select(customer => new CustomerSummaryDto
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    CreditScore = customer.CreditScore,
                    NumAccounts = customer.Accounts.Count(),
                    NumCreditCards = customer.CreditCards.Count()
                })
                .ToListAsync();
        }

        // 2
        /// Returns a list of customers whose credit score is greater than or equal to the specified minimum score.
        public async Task<List<CustomerSummaryDto>> GetCustomersByMinCreditScoreAsync(int minScore)
        {
            return await _context.Customer
                .Where(customer => customer.CreditScore > minScore)
                .Include(customer => customer.Accounts)
                .Include(customer => customer.CreditCards)
                .Select(customer => new CustomerSummaryDto
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    CreditScore = customer.CreditScore,
                    NumAccounts = customer.Accounts.Count(),
                    NumCreditCards = customer.CreditCards.Count()
                })
                .ToListAsync();
        }

        // 3
        /// Returns the total balance grouped by account type.
        public async Task<List<AccountBalanceDto>> GetTotalBalanceByAccountTypeAsync()
        {
            return await _context.Account
                .GroupBy(account => account.Type)
                .Select(group => new AccountBalanceDto
                {
                    AccountType = group.Key,
                    TotalBalance = group.Sum(a => a.Balance)
                })
                .ToListAsync();
        }

        // 4
        /// Returns a dictionary grouping customers by their state.
        public async Task<Dictionary<string, List<Customer>>> GetCustomersGroupedByStateAsync()
        {
            return await _context.Customer
                .GroupBy(customer => customer.State)
                .ToDictionaryAsync(
                    group => group.Key,
                    group => group.ToList()
                );
        }

        // 5
        /// Returns the number of customers in each state, ordered by count descending.
        public async Task<List<StateCustomerCountDto>> GetCustomerCountsByStateAsync()
        {
            return await _context.Customer
                .GroupBy(customer => customer.State)
                .Select(group => new StateCustomerCountDto
                {
                    State = group.Key,
                    CustomerCount = group.Count()
                })
                .OrderByDescending(customer => customer.CustomerCount)
                .ToListAsync();
        }

        // 6
        /// Returns a list of customers who have at least one credit card with available credit greater than or equal to the specified amount.
        public async Task<List<CustomerSummaryDto>> GetCustomersWithHighCreditLimitAsync(decimal minCredit)
        {
            return await _context.Customer
                .Include(customer => customer.CreditCards)
                .Where(customer => customer.CreditCards.Any(n => n.CreditLimit >= minCredit))
                .Select(customer => new CustomerSummaryDto
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    CreditScore = customer.CreditScore,
                    NumAccounts = customer.Accounts.Count(),
                    NumCreditCards = customer.CreditCards.Count(),
                })
                .ToListAsync();
        }

        // 7
        /// Groups customers by the century in which they were born.
        /// Note that string interpolation will not work in the GroupBy. You will
        /// need to leverage the fact that LINQ will work on any enumerable type...
        public async Task<Dictionary<string, List<Customer>>> GetCustomersGroupedByBirthCenturyAsync()
        {
            var customers = await _context.Customer.ToListAsync();
            return customers
                .GroupBy(customer => $"{customer.DOB.Year / 100 + 1}th century")
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList()
                );
        }

        // 8
        /// Groups customers into credit score brackets (e.g., "600–649") and returns a list of full names for each bracket.
        public async Task<Dictionary<string, List<string>>> GetCustomerCreditScoreBracketsAsync()
        {
            var projectedCustomers = await _context.Customer
                .Select(customer => new CreditScoreBracket
                {
                    Bracket = (customer.CreditScore / 50) * 50,
                    Name = $"{customer.FirstName} {customer.LastName}"
                })
                .ToListAsync();
            return projectedCustomers
                .GroupBy(customer => customer.Bracket)
                .OrderBy(group => group.Key)
                .ToDictionary(
                    group => $"{group.Key}-{group.Key + 59}",
                    group => group.Select(customer => customer.Name).ToList()
                );
        }

        // 9
        /// Calculates the average balance of savings accounts created within a specified date range.
        public async Task<decimal> GetAverageSavingsBalanceInRangeAsync(DateTime start, DateTime end)
        {
            var result = await _context.Account
                .Where(account => account.DateCreated > start && account.DateCreated < end && account.Type == "Savings")
                .Select(account => account.Balance)
                .ToListAsync();
            return result.Count > 0 ? result.Average() : 0;
        }

        // 10
        /// Returns a list of customers who have a credit score above the specified threshold and at least one checking account with a balance above the given minimum.
        public async Task<List<Customer>> GetQualifiedCheckingCustomersAsync(decimal minBalance, int minCreditScore)
        {
            return await _context.Customer
                .Where(customer => customer.CreditScore > minCreditScore && customer.Accounts.Any(account => account.Type == "Checking" && account.Balance > minBalance))
                .ToListAsync();
        }
    }
}