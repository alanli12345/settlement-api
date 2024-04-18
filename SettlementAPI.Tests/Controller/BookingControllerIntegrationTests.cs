using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SettlementBookingAPI.Helpers;
using SettlementBookingAPI.Models.Entities;
using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Models.Responses;
using SettlementBookingAPI.Repositories;
using SettlementBookingAPI.Services;
using SettlementBookingAPI.Strategies;
using static SettlementBookingAPI.Constants.BookingConstants;
using static SettlementBookingAPI.Tests.Constants.TestConstants;

namespace SettlementBookingAPI.Tests
{
    public class BookingControllerIntegrationTests
    {
        private readonly Mock<ILogger<BookingController>> _loggerMock;
        private readonly Mock<ILogger<BookingRepositoryProxy>> _proxyLoggerMock;
        private readonly IMapper _mapperMock;
        private readonly BookingController _bookingController;
        private readonly BookingService _bookingService;
        private readonly BookingHelper _bookingHelper;
        private readonly BookingRepository _bookingRepository;
        private readonly BookingRepositoryProxy _bookingRepositoryProxy;
        private readonly BookingStrategy _bookingStrategy;
        private readonly List<BookingEntity> _bookings;

        private readonly DateTime bookingTime = DateTime.UtcNow.Date.AddHours(10);

        public BookingControllerIntegrationTests()
        {
            _loggerMock = new Mock<ILogger<BookingController>>();
            _proxyLoggerMock = new Mock<ILogger<BookingRepositoryProxy>>();
            _bookingHelper = new BookingHelper();
            _bookings = new List<BookingEntity>()
                {
                    new BookingEntity { Name = "John", BookingTime = bookingTime },
                    new BookingEntity { Name = "Bob", BookingTime = bookingTime },
                    new BookingEntity { Name = "Jane", BookingTime = bookingTime },
                    new BookingEntity { Name = "Trevor", BookingTime = bookingTime }
                };

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapperMock = mapperConfig.CreateMapper();

            _bookingRepository = new BookingRepository(_bookings);
            _bookingRepositoryProxy = new BookingRepositoryProxy(_bookingRepository, _proxyLoggerMock.Object);
            _bookingStrategy = new BookingStrategy(_bookingRepositoryProxy);
            _bookingService = new BookingService(_bookingRepositoryProxy, _bookingStrategy, _bookingHelper, _mapperMock);
            _bookingController = new BookingController(_loggerMock.Object, _bookingService);
        }

        [Fact]
        public async Task BookAppointment_ReturnsOkResult_WhenAppointmentIsSuccessfullyBooked()
        {
            // Arrange
            var request = new BookingRequest { Name = TestName, BookingTime = TestBookingTime };

            // Act
            var result = await _bookingController.BookAppointment(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var bookingResult = Assert.IsType<BookingResponse>(okResult.Value);

            Assert.True(bookingResult.Success);
            Assert.Equal(SuccessfulMessage, bookingResult.Message);
        }

        [Fact]
        public async Task BookAppointment_ReturnsBadRequest_WhenBookingTimeFormatIsInvalid()
        {
            // Arrange
            var request = new BookingRequest { Name = TestName, BookingTime = TestInvalidBookingTime };

            // Act
            var result = await _bookingController.BookAppointment(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(InvalidFormatMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task BookAppointment_ReturnsBadRequest_WhenBookingTimeIsOutsideBusinessHours()
        {
            // Arrange
            var request = new BookingRequest { Name = TestName, BookingTime = TestOutOfHoursTime };

            // Act
            var result = await _bookingController.BookAppointment(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(OutOfHoursMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task BookAppointment_ReturnsBadRequest_WhenNameIsEmpty()
        {
            // Arrange
            var request = new BookingRequest { Name = "", BookingTime = TestBookingTime };

            // Act
            var result = await _bookingController.BookAppointment(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(EmptyNameMessage, badRequestResult.Value);
        }


        [Fact]
        public async Task BookAppointment_ReturnsBadRequest_WhenNameIsNull()
        {
            // Arrange
            var request = new BookingRequest { Name = null, BookingTime = TestBookingTime };

            // Act
            var result = await _bookingController.BookAppointment(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(EmptyNameMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task BookAppointment_ReturnsConflict_WhenBookingMoreThanFourBookingsWithinSameHour()
        {
            // Arrange
            var request = new BookingRequest { Name = TestName, BookingTime = TestConflictBookingTime };

            // Act
            var result = await _bookingController.BookAppointment(request);

            // Assert
            var badRequestResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(UnavailableMessage, badRequestResult.Value);
        }
    }
}