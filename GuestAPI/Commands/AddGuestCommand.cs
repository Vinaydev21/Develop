using GuestAPI.Models;

namespace GuestAPI.Commands
{
    /// <summary>
    /// Represents a command for adding a new guest.
    /// </summary>
    public class AddGuestCommand
    {
        /// <summary>
        /// Gets or sets the guest information to be added.
        /// </summary>
        public Guest Guest { get; set; }
    }
}
