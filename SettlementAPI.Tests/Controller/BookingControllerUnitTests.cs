using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Services.Interfaces;

namespace SettlementBookingAPI.Tests
{
    public class BookingControllerUnitTests
    {
        private readonly Mock<ILogger<BookingController>> _loggerMock;
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly BookingController _bookingController;

        public BookingControllerUnitTests()
        {
            _loggerMock = new Mock<ILogger<BookingController>>();
            _bookingServiceMock = new Mock<IBookingService>();
            _bookingController = new BookingController(_loggerMock.Object, _bookingServiceMock.Object);
        }

        [Fact]
        public async Task GetBooking_ReturnsBadRequest_WhenNameIsNull()
        {
            // Act
            var result = await _bookingController.GetBookingsByName(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetBooking_ReturnsBadRequest_WhenNameIsEmptyString()
        {
            // Act
            var result = await _bookingController.GetBookingsByName("");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task BookAppointment_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _bookingController.ModelState.AddModelError("BookingTime", "Booking time is required.");

            // Act
            var result = await _bookingController.BookAppointment(new BookingRequest());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}