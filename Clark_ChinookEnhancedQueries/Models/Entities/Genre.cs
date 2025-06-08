using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_ChinookEnhancedQueries.Models.Entities;

public class Genre
{
    [Key]
    public int GenreId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}