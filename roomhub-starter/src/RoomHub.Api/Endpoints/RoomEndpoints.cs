using RoomHub.Api.Services;

namespace RoomHub.Api.Endpoints;

public record CreateRoomRequest(string? Name, int Capacity);

public record RoomResponse(Guid Id, string Name, int Capacity);

public record CreateBookingRequest(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

public record BookingCreatedResponse(Guid Id, Guid RoomId, string Status);

public static class RoomEndpoints
{
    public static void MapRoomEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/rooms").WithTags("Rooms");

        group.MapPost("/", (CreateRoomRequest request, RoomService service) =>
        {
            var result = service.Create(request.Name, request.Capacity);
            if (!result.IsSuccess)
                return result.Error.ToProblem();
            var room = result.Value!;
            return Results.Created(
                $"/api/rooms/{room.Id}",
                new RoomResponse(room.Id, room.Name, room.Capacity));
        });

        group.MapGet("/{id:guid}", (Guid id, RoomService service) =>
        {
            var result = service.Get(id);
            if (!result.IsSuccess)
                return result.Error.ToProblem();
            var room = result.Value!;
            return Results.Ok(new RoomResponse(room.Id, room.Name, room.Capacity));
        });

        group.MapPost("/{roomId:guid}/bookings", (Guid roomId, CreateBookingRequest request, BookingService service) =>
        {
            var result = service.Create(
                roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees);
            if (!result.IsSuccess)
                return result.Error.ToProblem();
            var booking = result.Value!;

            return Results.Created(
                $"/api/bookings/{booking.Id}",
                new BookingCreatedResponse(booking.Id, booking.RoomId, booking.Status.ToString()));
        });
    }
}
