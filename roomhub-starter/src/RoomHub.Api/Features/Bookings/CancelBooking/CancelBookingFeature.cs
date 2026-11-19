using RoomHub.Api.Endpoints;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed record CancelBookingRequest(Guid Id);
public sealed record CancelBookingCommand(Guid Id);
public sealed record CancelBookingResponse(Guid Id, string Status);

public sealed class CancelBookingHandler(BookingRepository bookings)
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

        return Result<CancelBookingResponse>.Success(new CancelBookingResponse(booking.Id, booking.Status.ToString()));
    }
}

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
