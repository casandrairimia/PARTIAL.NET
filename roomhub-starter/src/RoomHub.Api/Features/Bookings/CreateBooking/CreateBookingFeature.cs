using RoomHub.Api.Endpoints;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingRequest(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);
public sealed record CreateBookingCommand(Guid RoomId, string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);
public sealed record CreateBookingResponse(Guid Id, Guid RoomId, string Status);

public sealed class CreateBookingHandler(
    RoomRepository rooms,
    BookingRepository bookings,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>>
{
    public Result<CreateBookingResponse> Handle(CreateBookingCommand command)
    {
        var room = rooms.Get(command.RoomId);
        if (room is null)
            return Result<CreateBookingResponse>.Failure(Error.NotFound("RoomNotFound", "The room does not exist."));

        var result = Booking.Create(room, command.BookedBy, command.StartUtc,
            command.EndUtc, command.Attendees, bookings.ListForRoom(room.Id),
            timeProvider.GetUtcNow().UtcDateTime);
        if (result.IsSuccess)
            bookings.Add(result.Value);
        if (result.IsFailure)
            return Result<CreateBookingResponse>.Failure(result.Error);

        var booking = result.Value;
        return Result<CreateBookingResponse>.Success(
            new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString()));
    }
}

public static class CreateBookingEndpoint
{
    public static void MapCreateBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings",
            (Guid roomId, CreateBookingRequest request, CreateBookingHandler handler) =>
            {
                var result = handler.Handle(new CreateBookingCommand(roomId, request.BookedBy,
                    request.StartUtc, request.EndUtc, request.Attendees));
                if (result.IsFailure) return result.Error.ToProblem();

                return Results.Created($"/api/bookings/{result.Value.Id}", result.Value);
            });
    }
}
