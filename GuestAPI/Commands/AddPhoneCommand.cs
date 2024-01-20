namespace GuestAPI.Commands
{
    /// <summary>
    /// Represents a command for adding a phone number to an existing guest.
    /// </summary>
    public class AddPhoneCommand
    {
        /// <summary>
        /// Gets or sets the ID of the guest to whom the phone number will be added.
        /// </summary>
        public Guid GuestId { get; set; }

        /// <summary>
        /// Gets or sets the phone number to be added.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
