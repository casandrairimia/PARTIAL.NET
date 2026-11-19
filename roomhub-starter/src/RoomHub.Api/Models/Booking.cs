namespace RoomHub.Api.Models;

using RoomHub.Api.Models.Common;

public enum BookingStatus
{
    Active,
    Cancelled
}

public class Booking
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public string BookedBy { get; set; } = string.Empty;
    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public int Attendees { get; set; }
    public BookingStatus Status { get; set; }

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        IEnumerable<Booking> existingBookings,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
            return Result<Booking>.Failure(Error.Validation("InvalidBookedBy", "BookedBy is required."));

        if (endUtc <= startUtc)
            return Result<Booking>.Failure(Error.Validation("InvalidRange", "The end must be after the start."));

        if (startUtc < nowUtc)
            return Result<Booking>.Failure(Error.Validation("BookingStartsInPast", "A booking cannot start in the past."));

        if (endUtc - startUtc > TimeSpan.FromHours(4))
            return Result<Booking>.Failure(Error.Validation("BookingTooLong", "A booking cannot exceed 4 hours."));

        if (attendees <= 0)
            return Result<Booking>.Failure(Error.Validation("InvalidAttendees", "Attendees must be positive."));

        if (attendees > room.Capacity)
            return Result<Booking>.Failure(Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats."));

        if (existingBookings.Any(existing => existing.Status == BookingStatus.Active &&
            existing.StartUtc < endUtc && startUtc < existing.EndUtc))
            return Result<Booking>.Failure(Error.Conflict("RoomOccupied", "The room is occupied in this interval."));

        return Result<Booking>.Success(new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = bookedBy.Trim(),
            StartUtc = startUtc,
            EndUtc = endUtc,
            Attendees = attendees,
            Status = BookingStatus.Active
        });
    }

    public Result<bool> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return Result<bool>.Failure(Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled."));

        Status = BookingStatus.Cancelled;
        return Result<bool>.Success(true);
    }
}
