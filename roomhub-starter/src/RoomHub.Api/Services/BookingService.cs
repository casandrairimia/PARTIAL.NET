using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Services;

public class BookingService(RoomRepository rooms, BookingRepository bookings, TimeProvider timeProvider)
{
    public Result<Booking> Create(Guid roomId, string? bookedBy, DateTime startUtc, DateTime endUtc, int attendees)
    {
        var room = rooms.Get(roomId);
        if (room is null)
            return Result<Booking>.Failure(Error.NotFound("RoomNotFound", "The room does not exist."));

        var result = Booking.Create(
            room, bookedBy, startUtc, endUtc, attendees,
            bookings.ListForRoom(roomId), timeProvider.GetUtcNow().UtcDateTime);
        if (!result.IsSuccess) return result;
        bookings.Add(result.Value!);
        return result;
    }

    public Result<Booking> Get(Guid id)
    {
        var booking = bookings.Get(id);
        if (booking is null)
            return Result<Booking>.Failure(Error.NotFound("BookingNotFound", "The booking does not exist."));

        return Result<Booking>.Success(booking);
    }

    public Result<bool> Cancel(Guid id)
    {
        var result = Get(id);
        if (!result.IsSuccess) return Result<bool>.Failure(result.Error!);
        return result.Value!.Cancel();
    }
}
