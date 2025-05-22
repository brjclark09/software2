using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clark_ChinookMusicApp.Data;
using Clark_ChinookMusicApp.Models;

namespace Clark_ChinookMusicApp.Services;

public class SeedingService
{
    private readonly ApplicationDbContext _context;
    public SeedingService(ApplicationDbContext context)
    {
        _context = context;
    }
}