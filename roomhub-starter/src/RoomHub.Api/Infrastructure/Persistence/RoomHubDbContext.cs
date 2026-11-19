using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Models;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class RoomHubDbContext(DbContextOptions<RoomHubDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(room => room.Id);
            entity.Property(room => room.Name).IsRequired();
            entity.HasMany<Booking>().WithOne().HasForeignKey(booking => booking.RoomId);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(booking => booking.Id);
            entity.Property(booking => booking.BookedBy).IsRequired();
            entity.Property(booking => booking.Status).HasConversion<string>();
        });
    }
}
