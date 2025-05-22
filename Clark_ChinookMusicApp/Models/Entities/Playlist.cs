using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_ChinookMusicApp.Models.Entities;

public class Playlist
{
    [Key]
    public int PlaylistId { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}