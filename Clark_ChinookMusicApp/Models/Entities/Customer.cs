using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_ChinookMusicApp.Models.Entities;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    [ForeignKey("Employee")]
    public int SupportRepId { get; set; }
    public virtual Employee SupportRep { get; set; }
}