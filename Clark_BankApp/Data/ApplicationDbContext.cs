using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clark_BankApp.Data
{
    public class ApplicationDbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<CreditCard> CreditCard { get; set; }
        public DbSet<Customer> Customer { get; set; }

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
}