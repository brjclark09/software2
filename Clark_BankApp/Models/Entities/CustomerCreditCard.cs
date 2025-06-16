using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clark_BankApp.Models.Entities
{
    public class CustomerCreditCard
    {
        public int CustId { get; set; }
        public int CardId { get; set; }
    }
}