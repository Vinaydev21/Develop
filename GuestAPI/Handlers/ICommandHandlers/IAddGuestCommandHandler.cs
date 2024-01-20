using GuestAPI.Commands;

namespace GuestAPI.Handlers.ICommandHandlers
{
    /// <summary>
    /// Define an interface for the AddGuestCommandHandler
    /// </summary>
    public interface IAddGuestCommandHandler
    {
        CommandResult Handle(AddGuestCommand command);
    }
}
