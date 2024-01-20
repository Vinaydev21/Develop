using GuestAPI.Commands;
using GuestAPI.Data;
using GuestAPI.Handlers.ICommandHandlers;

namespace GuestAPI.Handlers.CommandHandlers
{
    /// <summary>
    /// Handles the command to update an existing guest in the database.
    /// </summary>
    public class UpdateGuestCommandHandler : IUpdateGuestCommandHandler
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGuestCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UpdateGuestCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles the command to update an existing guest.
        /// </summary>
        /// <param name="command">The command containing the guest ID and updated guest details.</param>
        public void Handle(UpdateGuestCommand command)
        {
            try
            {
                var existingGuest = _context.Guests.FirstOrDefault(g => g.Id == command.GuestId);
                if (existingGuest != null)
                {
                    existingGuest.Title = command.UpdatedGuest.Title;
                    existingGuest.FirstName = command.UpdatedGuest.FirstName;
                    existingGuest.LastName = command.UpdatedGuest.LastName;
                    existingGuest.BirthDate = command.UpdatedGuest.BirthDate;
                    existingGuest.Email = command.UpdatedGuest.Email;
                    existingGuest.PhoneNumbers = command.UpdatedGuest.PhoneNumbers;

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
