using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clark_BankApp.Models.Entities
{
    public class Customer
    {
    [Key]
    public int CustId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DOB { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public DateTime DateJoined { get; set; }
    public int CreditScore { get; set; }
    public ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}