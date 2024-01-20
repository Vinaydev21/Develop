using GuestAPI.Commands;
using GuestAPI.Data;
using GuestAPI.Handlers.ICommandHandlers;

namespace GuestAPI.Handlers.CommandHandlers
{
    /// <summary>
    /// Handles the command to add a phone number to an existing guest in the database.
    /// </summary>
    public class AddPhoneCommandHandler : IAddPhoneCommandHandler
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPhoneCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AddPhoneCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles the command to add a phone number to a guest.
        /// </summary>
        /// <param name="command">The command containing the guest ID and phone number.</param>
        /// <returns>A <see cref="CommandResult"/> indicating the success or failure of the command.</returns>
        public CommandResult Handle(AddPhoneCommand command)
        {
            try
            {
                var guest = _context.Guests.FirstOrDefault(g => g.Id == command.GuestId);
                if (guest == null)
                {
                    return new CommandResult { Success = false, ErrorMessage = "Guest not found." };
                }

                // Validation: Ensure phone number is not duplicated
                if (guest.PhoneNumbers != null && guest.PhoneNumbers.Contains(command.PhoneNumber))
                {
                    return new CommandResult { Success = false, ErrorMessage = "Phone number is duplicated." };
                }

                if (guest.PhoneNumbers == null)
                {
                    guest.PhoneNumbers = new List<string>();
                }

                guest.PhoneNumbers = guest.PhoneNumbers.Concat(new[] { command.PhoneNumber }).ToList();
                _context.SaveChanges();

                return new CommandResult { Success = true };
            }
            catch (Exception ex)
            {
                

                return new CommandResult
                {
                    Success = false,
                    ErrorMessage = "An unexpected error occurred while adding the phone number."
                };
            }
        }
    }
}
