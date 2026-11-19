namespace RoomHub.Api.Models;

public interface IRoomRepository
{
    void Add(Room room);
    Room? Get(Guid id);
}
