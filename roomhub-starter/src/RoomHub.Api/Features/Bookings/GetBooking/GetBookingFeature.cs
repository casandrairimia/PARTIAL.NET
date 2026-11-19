using RoomHub.Api.Endpoints;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed record GetBookingRequest(Guid Id);
public sealed record GetBookingQuery(Guid Id);
public sealed record GetBookingResponse(Guid Id, Guid RoomId, string BookedBy, DateTime StartUtc,
    DateTime EndUtc, int Attendees, string Status);

public sealed class GetBookingHandler(BookingRepository bookings)
    : IQueryHandler<GetBookingQuery, Result<GetBookingResponse>>
{
    public Result<GetBookingResponse> Handle(GetBookingQuery query)
    {
        var booking = bookings.Get(query.Id);
        return booking is null
            ? Result<GetBookingResponse>.Failure(Error.NotFound("BookingNotFound", "The booking does not exist."))
            : Result<GetBookingResponse>.Success(new GetBookingResponse(booking.Id, booking.RoomId,
                booking.BookedBy, booking.StartUtc, booking.EndUtc, booking.Attendees, booking.Status.ToString()));
    }
}

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
