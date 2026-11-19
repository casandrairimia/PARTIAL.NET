using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Models;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class EfBookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public void Add(Booking booking) => db.Bookings.Add(booking);

    public Booking? Get(Guid id) => db.Bookings.SingleOrDefault(booking => booking.Id == id);

    public List<Booking> ListForRoom(Guid roomId) => db.Bookings
        .Where(booking => booking.RoomId == roomId)
        .ToList();
}
