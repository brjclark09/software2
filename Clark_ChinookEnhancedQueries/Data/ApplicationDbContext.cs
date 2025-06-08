using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clark_ChinookEnhancedQueries.Models.Entities;

namespace Clark_ChinookEnhancedQueries.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Album> Album { get; set; }
    public DbSet<Artist> Artist { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<Genre> Genre { get; set; }
    public DbSet<Invoice> Invoice { get; set; }
    public DbSet<InvoiceLine> InvoiceLine { get; set; }
    public DbSet<MediaType> MediaType { get; set; }
    public DbSet<Playlist> Playlist { get; set; }
    public DbSet<Track> Track { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=music.db");
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Playlist>()
        .HasMany(p => p.Tracks)
        .WithMany(t => t.Playlists)
        .UsingEntity<Dictionary<string, object>>(
            "PlaylistTrack",
            right => right.HasOne<Track>().WithMany().HasForeignKey("TrackId"),
            left => left.HasOne<Playlist>().WithMany().HasForeignKey("PlaylistId"),
            join => {
                join.HasKey("PlaylistId", "TrackId");
                join.Property<int>("PlaylistId");
                join.Property<int>("TrackId");
            });

    }
}