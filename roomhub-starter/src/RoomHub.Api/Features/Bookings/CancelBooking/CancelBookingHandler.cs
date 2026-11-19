using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(IBookingRepository bookings, IUnitOfWork unitOfWork)
    : ICommandHandler<CancelBookingCommand, Result<CancelBookingResponse>>
{
    public Result<CancelBookingResponse> Handle(CancelBookingCommand command)
    {
        var booking = bookings.Get(command.Id);
        if (booking is null)
            return Result<CancelBookingResponse>.Failure(Error.NotFound("BookingNotFound", "The booking does not exist."));

        var result = booking.Cancel();
        if (result.IsFailure)
            return Result<CancelBookingResponse>.Failure(result.Error);

        unitOfWork.SaveChanges();
        return Result<CancelBookingResponse>.Success(new CancelBookingResponse(booking.Id, booking.Status.ToString()));
    }
}
