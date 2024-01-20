namespace GuestAPI.Queries
{

    /// <summary>
    /// Represents a query to retrieve a specific guest by their identifier.
    /// </summary>
    public class GetGuestQuery
    {
        /// <summary>
        /// Gets or sets the unique identifier of the guest.
        /// </summary>
        public Guid GuestId { get; set; }
    }
}
