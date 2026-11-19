using RoomHub.Api.Endpoints;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

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
