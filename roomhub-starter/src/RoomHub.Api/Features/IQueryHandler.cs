namespace RoomHub.Api.Features;

public interface IQueryHandler<in TQuery, TResult>
{
    TResult Handle(TQuery query);
}
