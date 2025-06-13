using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_BankApp.Models.Dtos
{
    public class CustomerSummaryDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CreditScore { get; set; }
        public int NumAccounts { get; set; }
        public int NumCreditCards { get; set; }
    }
}