using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_BankApp.Models.Dtos
{
    public class AccountBalanceDto
    {
        public string AccountType { get; set; }
        public decimal TotalBalance { get; set; }
    }
}