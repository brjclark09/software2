using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clark_ChinookEnhancedQueries.Models.Entities;

namespace Clark_ChinookEnhancedQueries.Models.Dtos;

public class TrackDetails
{
    public required string Track { get; set; }
    public required string Album { get; set; }
    public required string Artist { get; set; }
}
