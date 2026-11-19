namespace RoomHub.Api.Models;

public interface IBookingRepository
{
    void Add(Booking booking);
    Booking? Get(Guid id);
    List<Booking> ListForRoom(Guid roomId);
}
