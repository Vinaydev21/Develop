using GuestAPI.Data;
using GuestAPI.Handlers.IQueryHandlers;
using GuestAPI.Models;
using GuestAPI.Queries;

namespace GuestAPI.Handlers.QueryHandlers
{
    /// <summary>
    /// Handles the query to retrieve all guests from the database.
    /// </summary>
    public class GetAllGuestsQueryHandler : IGetAllGuestsQueryHandler
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllGuestsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public GetAllGuestsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles the query to retrieve all guests.
        /// </summary>
        /// <param name="query">The query object.</param>
        /// <returns>A collection of guests.</returns>
        public IEnumerable<Guest> Handle(GetAllGuestsQuery query)
        {
            return _context.Guests.ToList();
        }
    }
}
