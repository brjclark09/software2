using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}