namespace GuestAPI.Commands
{
    /// <summary>
    /// Represents the result of a command operation.
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the command operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets an error message in case of failure.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
