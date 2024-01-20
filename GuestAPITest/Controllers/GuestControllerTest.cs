using GuestAPI.Commands;
using GuestAPI.Controllers;
using GuestAPI.Handlers.ICommandHandlers;
using GuestAPI.Handlers.IQueryHandlers;
using GuestAPI.Models;
using GuestAPI.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GuestAPITest.Controllers
{
    public class GuestControllerTest
    {
        // Mocks for dependencies
        private readonly Mock<ILogger<GuestController>> _loggerMock;
        private readonly Mock<IAddGuestCommandHandler> _addGuestCommandHandlerMock;
        private readonly Mock<IUpdateGuestCommandHandler> _updateGuestCommandHandlerMock;
        private readonly Mock<IAddPhoneCommandHandler> _addPhoneCommandHandlerMock;
        private readonly Mock<IGetGuestQueryHandler> _getGuestQueryHandlerMock;
        private readonly Mock<IGetAllGuestsQueryHandler> _getAllGuestsQueryHandlerMock;

        // Controller instance to be tested
        private readonly GuestController _guestController;

        public GuestControllerTest()
        {
            // Initialize mocks
            _loggerMock = new Mock<ILogger<GuestController>>();
            _addGuestCommandHandlerMock = new Mock<IAddGuestCommandHandler>();
            _updateGuestCommandHandlerMock = new Mock<IUpdateGuestCommandHandler>();
            _addPhoneCommandHandlerMock = new Mock<IAddPhoneCommandHandler>();
            _getGuestQueryHandlerMock = new Mock<IGetGuestQueryHandler>();
            _getAllGuestsQueryHandlerMock = new Mock<IGetAllGuestsQueryHandler>();

            // Initialize controller with mocks
            _guestController = new GuestController(
                _loggerMock.Object,
                _addGuestCommandHandlerMock.Object,
                _updateGuestCommandHandlerMock.Object,
                _addPhoneCommandHandlerMock.Object,
                _getGuestQueryHandlerMock.Object,
                _getAllGuestsQueryHandlerMock.Object
            );
        }

        [Fact]
        public void AddGuest_ValidGuest_ReturnsCreatedAtAction()
        {
            // Arrange
            var guest = new Guest
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                PhoneNumbers = new List<string> { "123456789" }
            };

            _addGuestCommandHandlerMock
                .Setup(h => h.Handle(It.IsAny<AddGuestCommand>()))
                .Returns(new CommandResult { Success = true })
                .Verifiable();

            // Act
            var result = _guestController.AddGuest(guest);

            // Assert
            var createdAtActionResult = Assert.IsType<ActionResult<Guest>>(result);
            Assert.IsType<CreatedAtActionResult>(createdAtActionResult.Result);
            _addGuestCommandHandlerMock.Verify();
        }
      
        [Fact]
        public void AddPhone_ValidData_ReturnsOk()
        {
            // Arrange
            var guestId = Guid.NewGuid();
            var phoneNumber = "123456789";

            _addPhoneCommandHandlerMock.Setup(h => h.Handle(It.IsAny<AddPhoneCommand>()))
                .Returns(new CommandResult { Success = true })
                .Verifiable();

            // Act
            var result = _guestController.AddPhone(guestId, phoneNumber);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Guest>>(result);
            Assert.IsType<OkResult>(actionResult.Result);        
            _addPhoneCommandHandlerMock.Verify();
        }

        [Fact]
        public void AddPhone_NullPhoneNumber_ReturnsBadRequest()
        {
            // Arrange
            var guestId = Guid.NewGuid();
            string phoneNumber = null; // Null phone number

            // Act
            var result = _guestController.AddPhone(guestId, phoneNumber);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Guest>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            _addPhoneCommandHandlerMock.Verify();
        }

        [Fact]
        public void GetGuestById_ExistingGuest_ReturnsOkResult()
        {
            // Arrange
            var guestId = Guid.NewGuid();
            var existingGuest = new Guest { Id = guestId, FirstName = "John", LastName = "Doe" };

            _getGuestQueryHandlerMock.Setup(h => h.Handle(It.IsAny<GetGuestQuery>()))
                .Returns(existingGuest);

            // Act
            var result = _guestController.GetGuestById(guestId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Guest>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult.Value);
            var returnedGuest = Assert.IsType<Guest>(okResult.Value);
            Assert.Equal(existingGuest.Id, returnedGuest.Id);
            Assert.Equal(existingGuest.FirstName, returnedGuest.FirstName);
            Assert.Equal(existingGuest.LastName, returnedGuest.LastName);
            _getGuestQueryHandlerMock.Verify();
        }

        [Fact]
        public void GetGuestById_NonExistingGuest_ReturnsNotFoundResult()
        {
            // Arrange
            var guestId = Guid.NewGuid();

            _getGuestQueryHandlerMock.Setup(h => h.Handle(It.IsAny<GetGuestQuery>()))
                .Returns((Guest)null); // non-existing guest

            // Act
            var result = _guestController.GetGuestById(guestId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Guest>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);       
            _getGuestQueryHandlerMock.Verify();
        }

        [Fact]
        public void GetGuestById_Exception_ReturnsStatusCode500Result()
        {
            // Arrange
            var guestId = Guid.NewGuid();

            _getGuestQueryHandlerMock.Setup(h => h.Handle(It.IsAny<GetGuestQuery>()))
                .Throws(new Exception("Simulated exception")); // Simulate an exception

            // Act
            var result = _guestController.GetGuestById(guestId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Guest>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);

            Assert.Equal(500, statusCodeResult.StatusCode);            
            _getGuestQueryHandlerMock.Verify();
        }

        [Fact]
        public void GetAllGuests_ReturnsOkResultWithGuestList()
        {
            // Arrange
            var guests = new List<Guest>
            {
                 new Guest { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
                 new Guest { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
            };

            _getAllGuestsQueryHandlerMock.Setup(h => h.Handle(It.IsAny<GetAllGuestsQuery>()))
                .Returns(guests);

            // Act
            var result = _guestController.GetAllGuests();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Guest>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult.Value);
            var returnedGuests = Assert.IsType<List<Guest>>(okResult.Value);
            Assert.Equal(guests.Count, returnedGuests.Count);          
            _getAllGuestsQueryHandlerMock.Verify();
        }

        [Fact]
        public void GetAllGuests_EmptyList_ReturnsOkResultWithEmptyList()
        {
            // Arrange
            _getAllGuestsQueryHandlerMock.Setup(h => h.Handle(It.IsAny<GetAllGuestsQuery>()))
                .Returns(new List<Guest>());

            // Act
            var result = _guestController.GetAllGuests();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Guest>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult.Value);
            var returnedGuests = Assert.IsType<List<Guest>>(okResult.Value);
            Assert.Empty(returnedGuests);            
            _getAllGuestsQueryHandlerMock.Verify();
        }

        [Fact]
        public void GetAllGuests_Exception_ReturnsStatusCode500Result()
        {
            // Arrange
            _getAllGuestsQueryHandlerMock.Setup(h => h.Handle(It.IsAny<GetAllGuestsQuery>()))
                .Throws(new Exception("Simulated exception")); // Simulate an exception

            // Act
            var result = _guestController.GetAllGuests();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Guest>>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);

            Assert.Equal(500, statusCodeResult.StatusCode);        
            _getAllGuestsQueryHandlerMock.Verify();
        }
    }

}

