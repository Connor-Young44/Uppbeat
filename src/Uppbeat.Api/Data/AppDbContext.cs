using Microsoft.EntityFrameworkCore;
using Uppbeat.Api.Models;

namespace Uppbeat.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) {}
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;
    public DbSet<TrackGenre> TrackGenres { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        mb.Entity<User>().HasIndex(u => u.Email).IsUnique();
        mb.Entity<Track>().HasMany(t => t.Genres).WithOne(g => g.Track).HasForeignKey(g => g.TrackId);
        mb.Entity<Track>()
            .HasOne(t => t.Owner)
            .WithMany(u => u.Tracks)
            .HasForeignKey(t => t.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}