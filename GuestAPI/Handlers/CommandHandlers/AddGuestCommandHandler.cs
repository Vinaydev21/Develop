using GuestAPI.Commands;
using GuestAPI.Data;
using GuestAPI.Handlers.ICommandHandlers;

namespace GuestAPI.Handlers.CommandHandlers
{
    /// <summary>
    /// Handles the command to add a new guest to the database.
    /// </summary>
    public class AddGuestCommandHandler : IAddGuestCommandHandler
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddGuestCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AddGuestCommandHandler(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Handles the command to add a new guest.
        /// </summary>
        /// <param name="command">The command containing the guest information.</param>
        /// <returns>A <see cref="CommandResult"/> indicating the success or failure of the command.</returns>
        public CommandResult Handle(AddGuestCommand command)
        {
            try
            {
                command.Guest.Id = Guid.NewGuid();
                _context.Guests.Add(command.Guest);
                _context.SaveChanges();

                return new CommandResult { Success = true };
            }
            catch(Exception ex)
            {
                

                return new CommandResult
                {
                    Success = false,
                    ErrorMessage = "An unexpected error occurred while adding the guest."
                };
            }
        }
    }
}
