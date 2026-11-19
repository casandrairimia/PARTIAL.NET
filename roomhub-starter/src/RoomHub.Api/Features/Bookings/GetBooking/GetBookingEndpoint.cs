using RoomHub.Api.Endpoints;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public static class GetBookingEndpoint
{
    public static void MapGetBooking(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings/{id:guid}", ([AsParameters] GetBookingRequest request, GetBookingHandler handler) =>
        {
            var result = handler.Handle(new GetBookingQuery(request.Id));
            if (result.IsFailure) return result.Error.ToProblem();
            return Results.Ok(result.Value);
        });
    }
}
