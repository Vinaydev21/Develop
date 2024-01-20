using GuestAPI.Commands;
using GuestAPI.Handlers.ICommandHandlers;
using GuestAPI.Handlers.IQueryHandlers;
using GuestAPI.Models;
using GuestAPI.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GuestAPI.Controllers
{
    /// <summary>
    /// Controller for managing guest-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Enforce API key authentication
    public class GuestController : ControllerBase
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IAddGuestCommandHandler _addGuestCommandHandler;
        private readonly IUpdateGuestCommandHandler _updateGuestCommandHandler;
        private readonly IAddPhoneCommandHandler _addPhoneCommandHandler;
        private readonly IGetGuestQueryHandler _getGuestQueryHandler;
        private readonly IGetAllGuestsQueryHandler _getAllGuestsQueryHandler;

        public GuestController(
            ILogger<GuestController> logger,
            IAddGuestCommandHandler addGuestCommandHandler,
            IUpdateGuestCommandHandler updateGuestCommandHandler,
            IAddPhoneCommandHandler addPhoneCommandHandler,
            IGetGuestQueryHandler getGuestQueryHandler,
            IGetAllGuestsQueryHandler getAllGuestsQueryHandler)
        {
            _logger = logger;
            _addGuestCommandHandler = addGuestCommandHandler;
            _updateGuestCommandHandler = updateGuestCommandHandler;
            _addPhoneCommandHandler = addPhoneCommandHandler;
            _getGuestQueryHandler = getGuestQueryHandler;
            _getAllGuestsQueryHandler = getAllGuestsQueryHandler;
        }


        /// <summary>
        /// Adds a new guest.
        /// </summary>
        /// <param name="command">The command to add a new guest.</param>
        /// <returns>
        /// The result of the operation. Returns 201 Created on success.
        /// Returns 400 Bad Request if the command is null or empty.
        /// Returns 500 Internal Server Error if an unexpected error occurs.
        /// </returns>
        [HttpPost]
        public ActionResult<Guest> AddGuest([FromBody] Guest command)
        {
            _logger.LogInformation("AddGuest endpoint called.");
            try
            {
                if (command == null)
                {
                    _logger.LogError("Guest data is null.");
                    return BadRequest("Guest data is null");
                }
                // Validation: At least one name and one phone number should be provided
                if (string.IsNullOrWhiteSpace(command.FirstName) && string.IsNullOrWhiteSpace(command.LastName))
                {
                    _logger.LogError("At least one name (FirstName or LastName) is required.");
                    return BadRequest("At least one name (FirstName or LastName) is required.");
                }

                if (command.PhoneNumbers == null || !command.PhoneNumbers.Any())
                {
                    _logger.LogError("At least one phone number is required.");
                    return BadRequest("At least one phone number is required.");
                }

                var addGuestCommand = new AddGuestCommand { Guest = command };
                _addGuestCommandHandler.Handle(addGuestCommand);

                _logger.LogInformation($"Guest added successfully. GuestId: {command.Id}");
                return CreatedAtAction(nameof(GetGuestById), new { id = command.Id }, command);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while processing AddGuest request");
                return StatusCode(500, "An unexpected error occurred");
            }                
        }

        /// <summary>
        /// Adds a phone number for an existing guest.
        /// </summary>
        /// <param name="id">The ID of the guest to add a phone number to.</param>
        /// <param name="phoneNumber">The phone number to add.</param>
        /// <returns>
        /// Returns 200 OK if the phone number is added successfully.
        /// Returns 400 Bad Request if the phone number is null or empty.
        /// Returns 500 Internal Server Error if an unexpected error occurs.
        /// </returns>
        [HttpPost("{id}/AddPhone")]
        public ActionResult<Guest> AddPhone(Guid id, [FromBody] string phoneNumber)
        {
            _logger.LogInformation("AddPhone endpoint called.");

            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    _logger.LogError("Phone number cannot be null or empty.");
                    return BadRequest("Phone number cannot be null or empty.");
                }

                var addPhoneCommand = new AddPhoneCommand { GuestId = id, PhoneNumber = phoneNumber };
                var result = _addPhoneCommandHandler.Handle(addPhoneCommand);

                if (result.Success)
                {
                    _logger.LogInformation($"Phone number added successfully. GuestId: {id}");
                    return Ok();
                }
                else
                {
                    _logger.LogError($"Error adding phone number: {result.ErrorMessage}");
                    return BadRequest(result.ErrorMessage);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error while adding phone number");
                return StatusCode(500, "An unexpected error occurred");

            }     
        }

        /// <summary>
        /// Gets a guest by their ID.
        /// </summary>
        /// <param name="id">The ID of the guest to retrieve.</param>
        /// <returns>
        /// The guest information. Returns 200 OK on success, or 404 Not Found if the guest is not found.
        /// Returns 500 Internal Server Error if an unexpected error occurs.        
        /// </returns>
        [HttpGet("{id}")]
        public ActionResult<Guest> GetGuestById(Guid id)
        {
            _logger.LogInformation($"GetGuestById endpoint called. GuestId: {id}");
            try
            {
                var getGuestQuery = new GetGuestQuery { GuestId = id };
                var guest = _getGuestQueryHandler.Handle(getGuestQuery);

                if (guest == null)
                {
                    _logger.LogError("Guest with ID {GuestId} not found", id);
                    return NotFound("Guest not found");
                }
                _logger.LogInformation($"Guest found successfully. GuestId: {guest.Id}");
                return Ok(guest);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while processing GetGuestById request");
                return StatusCode(500, "An unexpected error occurred");
            }
        }


        /// <summary>
        /// Gets all guests.
        /// </summary>
        /// <returns>
        /// The list of all guests. Returns 200 OK on success.
        /// Returns 500 Internal Server Error if an unexpected error occurs.        
        /// </returns>
        [HttpGet]
        public ActionResult<IEnumerable<Guest>> GetAllGuests()
        {
            _logger.LogInformation("GetAllGuests endpoint called.");
            try
            {
                var getAllGuestsQuery = new GetAllGuestsQuery();
                var guests = _getAllGuestsQueryHandler.Handle(getAllGuestsQuery).ToList();

                _logger.LogInformation($"Total guests retrieved: {guests.Count}");

                return Ok(guests);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while processing GetAllGuests request");                
                return StatusCode(500, "An unexpected error occurred");
            }       
        }


    }
}
