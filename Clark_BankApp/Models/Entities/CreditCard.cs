using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clark_BankApp.Models.Entities
{
    public class CreditCard
    {
    [Key]
    public int CardId { get; set; }
    public string CardNumber { get; set; }
    public string CardType { get; set; }
    public DateTime ExpDate { get; set; }
    public decimal AvailCredit { get; set; }
    public decimal CreditLimit { get; set; }
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}