using RoomHub.Api.Endpoints;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed record GetRoomRequest(Guid Id);
public sealed record GetRoomQuery(Guid Id);
public sealed record GetRoomResponse(Guid Id, string Name, int Capacity);

public sealed class GetRoomHandler(RoomRepository rooms)
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
