using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed class GetRoomHandler(IRoomRepository rooms)
    : IQueryHandler<GetRoomQuery, Result<GetRoomResponse>>
{
    public Result<GetRoomResponse> Handle(GetRoomQuery query)
    {
        var room = rooms.Get(query.Id);
        return room is null
            ? Result<GetRoomResponse>.Failure(Error.NotFound("RoomNotFound", "The room does not exist."))
            : Result<GetRoomResponse>.Success(new GetRoomResponse(room.Id, room.Name, room.Capacity));
    }
}
