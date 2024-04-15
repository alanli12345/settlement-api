using Moq;
using SettlementBookingAPI.Constants;
using SettlementBookingAPI.Helpers.Interfaces;
using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Repositories.Interfaces;
using SettlementBookingAPI.Services;
using SettlementBookingAPI.Services.Interfaces;
using SettlementBookingAPI.Strategies.Interfaces;
using static SettlementBookingAPI.Tests.Constants.TestConstants;

namespace SettlementBookingAPI.Tests.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepositoryProxy> _bookingRepositoryProxyMock;
        private readonly Mock<IBookingStrategy> _bookingStrategyMock;
        private readonly Mock<IBookingHelper> _bookingHelperMock;
        private readonly IBookingService _bookingService;

        public BookingServiceTests()
        {
            _bookingRepositoryProxyMock = new Mock<IBookingRepositoryProxy>();
            _bookingStrategyMock = new Mock<IBookingStrategy>();
            _bookingHelperMock = new Mock<IBookingHelper>();
            _bookingService = new BookingService(_bookingRepositoryProxyMock.Object, _bookingStrategyMock.Object, _bookingHelperMock.Object);
        }

        [Fact]
        public async Task BookAppointmentAsync_ReturnsErrorResponse_When_NameIsNull()
        {
            // Arrange
            var request = new BookingRequest { Name = null };

            // Act
            var result = await _bookingService.BookAppointmentAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(BookingConstants.EmptyNameMessage, result.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_ReturnsErrorResponse_When_BookingTimeIsInvalid()
        {
            // Arrange
            var request = new BookingRequest { Name = TestName, BookingTime = "invalid" };
            _bookingHelperMock.Setup(x => x.ParseBookingTime(request.BookingTime)).Returns((DateTime?)null);

            // Act
            var result = await _bookingService.BookAppointmentAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(BookingConstants.InvalidFormatMessage, result.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_ReturnsErrorResponse_When_BookingTimeIsOutOfHours()
        {
            // Arrange
            var request = new BookingRequest { Name = TestName, BookingTime = "18:00" };
            var bookingTime = DateTime.UtcNow;
            _bookingHelperMock.Setup(x => x.ParseBookingTime(request.BookingTime)).Returns(bookingTime);
            _bookingHelperMock.Setup(x => x.IsValidBookingTime(bookingTime)).Returns(false);

            // Act
            var result = await _bookingService.BookAppointmentAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(BookingConstants.OutOfHoursMessage, result.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_ReturnsErrorResponse_When_BookingIsNotAvailable()
        {
            // Arrange
            var request = new BookingRequest { Name = TestName, BookingTime = "10:00" };
            var bookingTime = DateTime.UtcNow;
            _bookingHelperMock.Setup(x => x.ParseBookingTime(request.BookingTime)).Returns(bookingTime);
            _bookingHelperMock.Setup(x => x.IsValidBookingTime(bookingTime)).Returns(true);
            _bookingStrategyMock.Setup(x => x.CanBookAsync(request.Name, bookingTime)).ReturnsAsync(false);

            // Act
            var result = await _bookingService.BookAppointmentAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(BookingConstants.UnavailableMessage, result.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_ReturnsSuccessResponse_When_BookingIsSuccessful()
        {
            // Arrange
            var request = new BookingRequest { Name = TestName, BookingTime = "10:00" };
            var bookingTime = DateTime.UtcNow;
            var bookingId = Guid.NewGuid();
            _bookingHelperMock.Setup(x => x.ParseBookingTime(request.BookingTime)).Returns(bookingTime);
            _bookingHelperMock.Setup(x => x.IsValidBookingTime(bookingTime)).Returns(true);
            _bookingStrategyMock.Setup(x => x.CanBookAsync(request.Name, bookingTime)).ReturnsAsync(true);
            _bookingRepositoryProxyMock.Setup(x => x.AddBookingAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask);

            // Act
            var result = await _bookingService.BookAppointmentAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(BookingConstants.SuccessfulMessage, result.Message);
            Assert.NotEqual(Guid.Empty, result.BookingId);
        }

        [Fact]
        public async Task GetBookings_ReturnsErrorResponse_When_NameIsNull()
        {
            // Act
            var result = await _bookingService.GetBookings(null);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(BookingConstants.EmptyNameMessage, result.Message);
        }

        [Fact]
        public async Task GetBookings_ReturnsBookings_When_NameIsValid()
        {
            // Arrange
            var name = TestName;
            var bookings = new List<Booking> { new Booking { BookingId = Guid.NewGuid(), Name = name, BookingTime = DateTime.UtcNow } };
            _bookingRepositoryProxyMock.Setup(x => x.GetBookingsByNameAsync(name)).ReturnsAsync(bookings);

            // Act
            var result = await _bookingService.GetBookings(name);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(BookingConstants.RetrievedMessage, result.Message);
            Assert.Equal(bookings, result.Bookings);
        }
    }
}
