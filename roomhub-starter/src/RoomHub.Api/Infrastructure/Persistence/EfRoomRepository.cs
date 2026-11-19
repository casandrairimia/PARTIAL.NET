using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Models;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class EfRoomRepository(RoomHubDbContext db) : IRoomRepository
{
    public void Add(Room room) => db.Rooms.Add(room);

    public Room? Get(Guid id) => db.Rooms.SingleOrDefault(room => room.Id == id);
}
