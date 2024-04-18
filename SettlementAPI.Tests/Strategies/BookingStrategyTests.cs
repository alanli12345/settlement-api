using Moq;
using SettlementBookingAPI.Models.Entities;
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
            _bookingRepositoryProxyMock.Setup(x => x.GetBookingsByNameAsync(TestName)).ReturnsAsync(new List<BookingEntity>());

            // Act
            var result = await _bookingStrategy.CanBookAsync(TestName, bookingTime);

            // Assert
            Assert.True(result);
        }

        private IEnumerable<BookingEntity> GetMockBookingsAtTime(int count)
        {
            var bookings = new List<BookingEntity>();
            for (int i = 0; i < count; i++)
            {
                bookings.Add(new BookingEntity { BookingTime = DateTime.UtcNow });
            }
            return bookings;
        }

        private IEnumerable<BookingEntity> GetMockUserBookings(DateTime bookingTime)
        {
            return new List<BookingEntity> { new BookingEntity { BookingTime = bookingTime } };
        }
    }
}
