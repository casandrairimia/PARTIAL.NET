namespace RoomHub.Api.Models;

using RoomHub.Api.Models.Common;

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }

    public static Result<Room> Create(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Room>.Failure(Error.Validation("InvalidName", "Room name is required."));

        if (capacity <= 0)
            return Result<Room>.Failure(Error.Validation("InvalidCapacity", "Capacity must be positive."));

        return Result<Room>.Success(new Room
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Capacity = capacity
        });
    }
}
