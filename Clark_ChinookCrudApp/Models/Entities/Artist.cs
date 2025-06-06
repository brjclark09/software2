using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_ChinookCrudApp.Models.Entities;

public class Artist
{
    [Key]
    public int ArtistId { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
}