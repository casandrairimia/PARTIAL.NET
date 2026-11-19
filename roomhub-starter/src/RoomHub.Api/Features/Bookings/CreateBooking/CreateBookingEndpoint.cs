using RoomHub.Api.Endpoints;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public static void MapCreateBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings",
            (Guid roomId, CreateBookingRequest request, CreateBookingHandler handler) =>
            {
                var command = new CreateBookingCommand(roomId, request.BookedBy,
                    request.StartUtc, request.EndUtc, request.Attendees);
                var result = handler.Handle(command);
                if (result.IsFailure) return result.Error.ToProblem();
                return Results.Created($"/api/bookings/{result.Value.Id}", result.Value);
            });
    }
}
