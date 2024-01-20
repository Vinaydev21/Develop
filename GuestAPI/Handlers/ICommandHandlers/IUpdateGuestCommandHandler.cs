using GuestAPI.Commands;

namespace GuestAPI.Handlers.ICommandHandlers
{
    /// <summary>
    /// Define an interface for the IUpdateGuestCommandHandler
    /// </summary>
    public interface IUpdateGuestCommandHandler
    {
        void Handle(UpdateGuestCommand command);
    }
}
