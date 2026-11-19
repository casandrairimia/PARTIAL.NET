using RoomHub.Api.Endpoints;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public static class GetRoomEndpoint
{
    public static void MapGetRoom(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rooms/{id:guid}", ([AsParameters] GetRoomRequest request, GetRoomHandler handler) =>
        {
            var result = handler.Handle(new GetRoomQuery(request.Id));
            if (result.IsFailure) return result.Error.ToProblem();
            return Results.Ok(result.Value);
        });
    }
}
