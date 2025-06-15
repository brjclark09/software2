using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var result = await _context.Customer
                .GroupJoin(
                    _context.Account,
                    customer => customer.CustomerId,
                    account => account.CustomerId,
                    (customer, account) => new CustomerSummaryDto
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        CreditScore = customer.CreditScore,
                        NumAccounts = account.Count(),
                        NumCreditCards = 
                    }
                )
                .ToListAsync();
            return result;
        }

        // 2
        /// Returns a list of customers whose credit score is greater than or equal to the specified minimum score.
        public async Task<List<CustomerSummaryDto>> GetCustomersByMinCreditScoreAsync(int minScore)
        {
            var result = await _context.Customer

            return result;
        }

        // 3
        /// Returns the total balance grouped by account type.
        public async Task<List<AccountBalanceDto>> GetTotalBalanceByAccountTypeAsync()
        {
            var result = await _context.Customer

            return result;
        }

        // 4
        /// Returns a dictionary grouping customers by their state.
        public async Task<Dictionary<string, List<Customer>>> GetCustomersGroupedByStateAsync()
        {
            var result = await _context.Customer

            return result;
        }

        // 5
        /// Returns the number of customers in each state, ordered by count descending.
        public async Task<List<StateCustomerCountDto>> GetCustomerCountsByStateAsync()
        {
            var result = await _context.Customer

            return result;
        }

        // 6
        /// Returns a list of customers who have at least one credit card with available credit greater than or equal to the specified amount.
        public async Task<List<CustomerSummaryDto>> GetCustomersWithHighCreditLimitAsync(decimal minCredit)
        {
            var result = await _context.Customer

            return result;
        }

        // 7
        /// Groups customers by the century in which they were born.
        /// Note that string interpolation will not work in the GroupBy. You will
        /// need to leverage the fact that LINQ will work on any enumerable type...
        public async Task<Dictionary<string, List<Customer>>> GetCustomersGroupedByBirthCenturyAsync()
        {
            var result = await _context.Customer

            return result;
        }

        // 8
        /// Groups customers into credit score brackets (e.g., "600–649") and returns a list of full names for each bracket.
        public async Task<Dictionary<string, List<string>>> GetCustomerCreditScoreBracketsAsync()
        {
            var result = await _context.Customer

            return result;
        }

        // 9
        /// Calculates the average balance of savings accounts created within a specified date range.
        public async Task<decimal> GetAverageSavingsBalanceInRangeAsync(DateTime start, DateTime end)
        {
            var result = await _context.Customer

            return result;
        }

        // 10
        /// Returns a list of customers who have a credit score above the specified threshold and at least one checking account with a balance above the given minimum.
        public async Task<List<Customer>> GetQualifiedCheckingCustomersAsync(decimal minBalance, int minCreditScore)
        {
            var result = await _context.Customer

            return result;
        }
    }
}