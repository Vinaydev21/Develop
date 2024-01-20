using GuestAPI.Models;

namespace GuestAPI.Commands
{
    /// <summary>
    /// Represents a command for updating guest information.
    /// </summary>
    public class UpdateGuestCommand
    {
        /// <summary>
        /// Gets or sets the ID of the guest to be updated.
        /// </summary>
        public Guid GuestId { get; set; }

        /// <summary>
        /// Gets or sets the updated guest information.
        /// </summary>
        public Guest UpdatedGuest { get; set; }
    }
}
