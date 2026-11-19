using RoomHub.Api.Services;

namespace RoomHub.Api.Endpoints;

public record BookingResponse(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    string Status);

public static class BookingEndpoints
{
    public static void MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/bookings").WithTags("Bookings");

        group.MapGet("/{id:guid}", (Guid id, BookingService service) =>
        {
            var result = service.Get(id);
            if (!result.IsSuccess)
                return result.Error.ToProblem();
            var b = result.Value!;
            return Results.Ok(new BookingResponse(
                b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status.ToString()));
        });

        group.MapPost("/{id:guid}/cancel", (Guid id, BookingService service) =>
        {
            var result = service.Cancel(id);
            if (!result.IsSuccess)
                return result.Error.ToProblem();
            return Results.NoContent();
        });
    }
}
