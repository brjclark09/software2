using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_ChinookCrudApp.Models.Entities;

public class MediaType
{
    [Key]
    public int MediaTypeId { get; set; }
    public string Name { get; set; }
}