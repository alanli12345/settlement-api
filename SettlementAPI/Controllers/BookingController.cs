using Microsoft.AspNetCore.Mvc;
using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Services.Interfaces;
using static SettlementBookingAPI.Constants.BookingConstants;

namespace SettlementBookingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;

        private const string GetBookingsErrorMessage = "Failed to fetch bookings, reason: ";
        private const string BookAppointmentErrorMessage = "Failed to book appointment, reason: ";
        public BookingController(ILogger<BookingController> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.LogError(GetBookingsErrorMessage + EmptyNameMessage); 
                return BadRequest(EmptyNameMessage);
            }

            var result = await _bookingService.GetBookings(name);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                _logger.LogError(GetBookingsErrorMessage + result.Message);
                return BadRequest(result.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(BookAppointmentErrorMessage + InvalidModelMessage);
                return BadRequest(ModelState);
            }

            var result = await _bookingService.BookAppointmentAsync(request);

            if (result.Success)
            {
                return Ok(result);
            }
            else if (result.IsConflict)
            {
                _logger.LogError(BookAppointmentErrorMessage + result.Message); 
                return Conflict(result.Message);
            }
            else
            {
                _logger.LogError(BookAppointmentErrorMessage + result.Message);
                return BadRequest(result.Message);
            }
        }
    }
}
