using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_ChinookEnhancedQueries.Models.Dtos
{
    public class CountryTransactionSummaryDto
    {
        public string? Name { get; set; }
        public int TransactionCount { get; set; }
        public decimal TransactionTotal { get; set; }
    }
}