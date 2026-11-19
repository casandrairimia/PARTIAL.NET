using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Services;

public class RoomService(RoomRepository rooms)
{
    public Result<Room> Create(string? name, int capacity)
    {
        var result = Room.Create(name, capacity);
        if (!result.IsSuccess) return result;
        rooms.Add(result.Value!);
        return result;
    }

    public Result<Room> Get(Guid id)
    {
        var room = rooms.Get(id);
        if (room is null)
            return Result<Room>.Failure(Error.NotFound("RoomNotFound", "The room does not exist."));

        return Result<Room>.Success(room);
    }
}
