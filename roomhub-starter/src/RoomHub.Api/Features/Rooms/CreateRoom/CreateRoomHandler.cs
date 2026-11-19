using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(IRoomRepository rooms, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>>
{
    public Result<CreateRoomResponse> Handle(CreateRoomCommand command)
    {
        var result = Room.Create(command.Name, command.Capacity);
        if (result.IsFailure)
            return Result<CreateRoomResponse>.Failure(result.Error);

        var room = result.Value;
        rooms.Add(room);
        unitOfWork.SaveChanges();
        return Result<CreateRoomResponse>.Success(new CreateRoomResponse(room.Id, room.Name, room.Capacity));
    }
}
