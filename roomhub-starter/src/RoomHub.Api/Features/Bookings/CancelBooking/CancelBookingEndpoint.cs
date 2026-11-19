using RoomHub.Api.Endpoints;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapCancelBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", ([AsParameters] CancelBookingRequest request, CancelBookingHandler handler) =>
        {
            var result = handler.Handle(new CancelBookingCommand(request.Id));
            if (result.IsFailure) return result.Error.ToProblem();
            return Results.NoContent();
        });
    }
}
