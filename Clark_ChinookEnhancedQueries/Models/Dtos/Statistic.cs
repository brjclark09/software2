using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_ChinookEnhancedQueries.Models.Dtos;
// Represents a single summary data point, such as an average or total.
public class Statistic
{
    // a short name describing what this statistic refers to (e.g., a genre name or category)
    public required string Label { get; set; }
    // the numeric value of the statistic
    public decimal Value { get; set; }
    // a unit of measurement for the value (e.g., "Seconds", "Count")
    public string? ValueMetric { get; set; }
}