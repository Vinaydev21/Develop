using GuestAPI.Models;
using GuestAPI.Queries;

namespace GuestAPI.Handlers.IQueryHandlers
{
    public interface IGetAllGuestsQueryHandler
    {
        IEnumerable<Guest> Handle(GetAllGuestsQuery query);
    }
}
