using SettlementBookingAPI.Models.Entities;
using SettlementBookingAPI.Repositories;

namespace SettlementBookingAPI.Tests.Repositories
{
    public class BookingRepositoryTests
    {
        private readonly BookingRepository _bookingRepository;
        private readonly List<BookingEntity> _bookings;
        private readonly DateTime testBookingTime = DateTime.UtcNow.Date.AddHours(10);

        public BookingRepositoryTests()
        {
            _bookings = new List<BookingEntity>
            {
                new BookingEntity { BookingId = Guid.NewGuid(), Name = "John", BookingTime = testBookingTime },
                new BookingEntity { BookingId = Guid.NewGuid(), Name = "James", BookingTime = testBookingTime },
                new BookingEntity { BookingId = Guid.NewGuid(), Name = "Bob", BookingTime = testBookingTime },
                new BookingEntity { BookingId = Guid.NewGuid(), Name = "Jane", BookingTime = testBookingTime }
            };

            _bookingRepository = new BookingRepository(_bookings);
        }

        [Fact]
        public async Task GetBookingsByNameAsync_ReturnsBookings_WhenNameMatches()
        {
            // Arrange
            string name = "John";

            // Act
            var bookings = await _bookingRepository.GetBookingsByNameAsync(name);

            // Assert
            Assert.NotNull(bookings);
            Assert.NotEmpty(bookings);
            Assert.All(bookings, b => Assert.Equal(name, b.Name, StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task GetBookingsByNameAsync_ReturnsEmpty_WhenNameDoesNotMatch()
        {
            // Arrange
            string name = "NonExistentName";

            // Act
            var bookings = await _bookingRepository.GetBookingsByNameAsync(name);

            // Assert
            Assert.NotNull(bookings);
            Assert.Empty(bookings);
        }

        [Fact]
        public async Task GetBookingsAtTimeAsync_ReturnsBookings_WhenBookingsExistAtSpecifiedTime()
        {
            // Arrange
            DateTime bookingTime = testBookingTime;

            // Act
            var bookings = await _bookingRepository.GetBookingsAtTimeAsync(bookingTime);

            // Assert
            Assert.NotNull(bookings);
            Assert.NotEmpty(bookings);
            Assert.All(bookings, b => Assert.True(b.BookingTime >= bookingTime && b.BookingTime < bookingTime.AddHours(1)));
        }

        [Fact]
        public async Task GetBookingsAtTimeAsync_ReturnsEmpty_WhenNoBookingsExistAtSpecifiedTime()
        {
            // Arrange
            DateTime bookingTime = DateTime.UtcNow.AddHours(-1); // A time before any bookings in the repository

            // Act
            var bookings = await _bookingRepository.GetBookingsAtTimeAsync(bookingTime);

            // Assert
            Assert.NotNull(bookings);
            Assert.Empty(bookings);
        }

        [Fact]
        public async Task AddBookingAsync_AddsBookingToRepository()
        {
            // Arrange
            var bookingToAdd = new BookingEntity { BookingId = Guid.NewGuid(), Name = "NewBooking", BookingTime = testBookingTime };

            // Act
            await _bookingRepository.AddBookingAsync(bookingToAdd);

            // Assert
            Assert.Contains(bookingToAdd, _bookings);
        }
    }
}
