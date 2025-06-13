using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_BankApp.Models.Entities
{
    public class Account
    {
    [Key]
    public int AccountId { get; set; }
    public string Type { get; set; }
    public DateTime DateCreated { get; set; }
    public decimal Balance { get; set; }
    }
}