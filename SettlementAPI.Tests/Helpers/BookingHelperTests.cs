using SettlementBookingAPI.Helpers;

namespace SettlementBookingAPI.Tests.Helpers
{
    public class BookingHelperTests
    {
        private readonly BookingHelper _bookingHelper;

        public BookingHelperTests()
        {
            _bookingHelper = new BookingHelper();
        }

        [Theory]
        [InlineData("08:59")] // Before 9 AM
        [InlineData("16:01")] // After 4 PM
        public void IsValidBookingTime_InvalidCases_ReturnsFalse(string bookingTime)
        {
            // Arrange
            DateTime parsedTime = DateTime.ParseExact(bookingTime, "HH:mm", null);

            // Act
            bool result = _bookingHelper.IsValidBookingTime(parsedTime);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("09:00")] // 9 AM
        [InlineData("15:59")] // 3:59 PM
        [InlineData("16:00")] // 4 PM
        public void IsValidBookingTime_ValidCases_ReturnsTrue(string bookingTime)
        {
            // Arrange
            DateTime parsedTime = DateTime.ParseExact(bookingTime, "HH:mm", null);

            // Act
            bool result = _bookingHelper.IsValidBookingTime(parsedTime);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("09:00", 9, 0)] // Valid time
        [InlineData("15:30", 15, 30)] // Valid time
        [InlineData("16:00", 16, 0)] // Last valid time
        public void ParseBookingTime_ValidCases_ReturnsParsedDateTime(string bookingTime, int expectedHour, int expectedMinute)
        {
            // Act
            DateTime? result = _bookingHelper.ParseBookingTime(bookingTime);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedHour, result.Value.Hour);
            Assert.Equal(expectedMinute, result.Value.Minute);
        }

        [Theory]
        [InlineData("25:00")] // Invalid hour
        [InlineData("15:61")] // Invalid minute
        public void ParseBookingTime_InvalidCases_ReturnsNull(string bookingTime)
        {
            // Act
            DateTime? result = _bookingHelper.ParseBookingTime(bookingTime);

            // Assert
            Assert.Null(result);
        }
    }
}
