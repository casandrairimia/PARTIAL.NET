using RoomHub.Api.Endpoints;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomRequest(string? Name, int Capacity);
public sealed record CreateRoomCommand(string? Name, int Capacity);
public sealed record CreateRoomResponse(Guid Id, string Name, int Capacity);

public sealed class CreateRoomHandler(RoomRepository rooms)
    : ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>>
{
    public Result<CreateRoomResponse> Handle(CreateRoomCommand command)
    {
        var result = Room.Create(command.Name, command.Capacity);
        if (result.IsFailure)
            return Result<CreateRoomResponse>.Failure(result.Error);

        var room = result.Value;
        rooms.Add(room);
        return Result<CreateRoomResponse>.Success(new CreateRoomResponse(room.Id, room.Name, room.Capacity));
    }
}

public static class CreateRoomEndpoint
{
    public static void MapCreateRoom(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", (CreateRoomRequest request, CreateRoomHandler handler) =>
        {
            var result = handler.Handle(new CreateRoomCommand(request.Name, request.Capacity));
            if (result.IsFailure) return result.Error.ToProblem();

            return Results.Created($"/api/rooms/{result.Value.Id}", result.Value);
        });
    }
}
