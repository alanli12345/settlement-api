using Moq;
using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Repositories.Interfaces;
using SettlementBookingAPI.Strategies;
using SettlementBookingAPI.Strategies.Interfaces;
using static SettlementBookingAPI.Tests.Constants.TestConstants;

namespace SettlementBookingAPI.Tests.Strategies
{
    public class BookingStrategyTests
    {
        private readonly Mock<IBookingRepositoryProxy> _bookingRepositoryProxyMock;
        private readonly IBookingStrategy _bookingStrategy;

        public BookingStrategyTests()
        {
            _bookingRepositoryProxyMock = new Mock<IBookingRepositoryProxy>();
            _bookingStrategy = new BookingStrategy(_bookingRepositoryProxyMock.Object);
        }

        [Fact]
        public async Task CanBookAsync_ReturnsFalse_When_BookingsAtSpecifiedTimeAreFull()
        {
            // Arrange
            var bookingTime = DateTime.UtcNow;
            _bookingRepositoryProxyMock.Setup(x => x.GetBookingsAtTimeAsync(bookingTime)).ReturnsAsync(GetMockBookingsAtTime(4));

            // Act
            var result = await _bookingStrategy.CanBookAsync(TestName, bookingTime);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CanBookAsync_ReturnsFalse_When_UserHasBookingAtSpecifiedTime()
        {
            // Arrange
            var bookingTime = DateTime.UtcNow;
            _bookingRepositoryProxyMock.Setup(x => x.GetBookingsAtTimeAsync(bookingTime)).ReturnsAsync(GetMockBookingsAtTime(3));
            _bookingRepositoryProxyMock.Setup(x => x.GetBookingsByNameAsync(TestName)).ReturnsAsync(GetMockUserBookings(bookingTime));

            // Act
            var result = await _bookingStrategy.CanBookAsync(TestName, bookingTime);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CanBookAsync_ReturnsTrue_When_BookingIsPossible()
        {
            // Arrange
            var bookingTime = DateTime.UtcNow;
            _bookingRepositoryProxyMock.Setup(x => x.GetBookingsAtTimeAsync(bookingTime)).ReturnsAsync(GetMockBookingsAtTime(3));
            _bookingRepositoryProxyMock.Setup(x => x.GetBookingsByNameAsync(TestName)).ReturnsAsync(new List<Booking>());

            // Act
            var result = await _bookingStrategy.CanBookAsync(TestName, bookingTime);

            // Assert
            Assert.True(result);
        }

        private IEnumerable<Booking> GetMockBookingsAtTime(int count)
        {
            var bookings = new List<Booking>();
            for (int i = 0; i < count; i++)
            {
                bookings.Add(new Booking { BookingTime = DateTime.UtcNow });
            }
            return bookings;
        }

        private IEnumerable<Booking> GetMockUserBookings(DateTime bookingTime)
        {
            return new List<Booking> { new Booking { BookingTime = bookingTime } };
        }
    }
}
