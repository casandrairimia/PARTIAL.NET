using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
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
        if (result.IsFailure)
            return Result<CreateBookingResponse>.Failure(result.Error);

        var booking = result.Value;
        bookings.Add(booking);
        unitOfWork.SaveChanges();
        return Result<CreateBookingResponse>.Success(
            new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString()));
    }
}
