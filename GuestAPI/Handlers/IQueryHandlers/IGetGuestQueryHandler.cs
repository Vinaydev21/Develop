using GuestAPI.Models;
using GuestAPI.Queries;

namespace GuestAPI.Handlers.IQueryHandlers
{
    public interface IGetGuestQueryHandler
    {
        Guest Handle(GetGuestQuery query);
    }
}
