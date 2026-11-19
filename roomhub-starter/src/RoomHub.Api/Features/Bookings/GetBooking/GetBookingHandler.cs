using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed class GetBookingHandler(IBookingRepository bookings)
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
