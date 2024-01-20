using GuestAPI.Data;
using GuestAPI.Handlers.IQueryHandlers;
using GuestAPI.Models;
using GuestAPI.Queries;

namespace GuestAPI.Handlers.QueryHandlers
{
    /// <summary>
    /// Handles the query to retrieve a specific guest from the database.
    /// </summary>
    public class GetGuestQueryHandler : IGetGuestQueryHandler
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGuestQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public GetGuestQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles the query to retrieve a specific guest.
        /// </summary>
        /// <param name="query">The query object.</param>
        /// <returns>
        /// The guest matching the specified ID, or <c>null</c> if not found.
        /// </returns>
        /// <remarks>
        /// If no guest is found with the specified ID, the method returns <c>null</c>.
        /// </remarks>
        public Guest Handle(GetGuestQuery query)
        {
            return _context.Guests.FirstOrDefault(g => g.Id == query.GuestId);
        }
    }
}
