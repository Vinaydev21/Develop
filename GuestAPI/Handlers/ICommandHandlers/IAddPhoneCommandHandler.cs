using GuestAPI.Commands;

namespace GuestAPI.Handlers.ICommandHandlers
{
    /// <summary>
    /// Define an interface for the IAddPhoneCommandHandler
    /// </summary>
    public interface IAddPhoneCommandHandler
    {
        CommandResult Handle(AddPhoneCommand command);
    }
}
