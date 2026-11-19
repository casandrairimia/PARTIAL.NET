namespace RoomHub.Api.Features;

public interface ICommandHandler<in TCommand, TResult>
{
    TResult Handle(TCommand command);
}
