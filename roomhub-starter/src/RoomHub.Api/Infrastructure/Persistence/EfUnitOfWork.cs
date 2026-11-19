using RoomHub.Api.Models;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class EfUnitOfWork(RoomHubDbContext db) : IUnitOfWork
{
    public void SaveChanges() => db.SaveChanges();
}
