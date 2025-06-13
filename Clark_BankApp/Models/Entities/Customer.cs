using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}