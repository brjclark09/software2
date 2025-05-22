using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_ChinookMusicApp.Models.Entities;

public class InvoiceLine
{
    [Key]
    public int InvoiceLineId { get; set; }
    [ForeignKey("Invoice")]
    public int InvoiceId { get; set; }
    public virtual Invoice Invoice { get; set; }
    public int TrackId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}