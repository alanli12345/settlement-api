using Microsoft.AspNetCore.Mvc;
using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class BaseController : ControllerBase
{
    protected readonly ILogger<BaseController> _logger;

    public BaseController(ILogger<BaseController> logger)
    {
        _logger = logger;
    }

    protected void LogWarning(string message)
    {
        _logger.LogWarning(message);
    }

    protected IActionResult HandleBadRequest(string message)
    {
        LogWarning(message);
        return BadRequest(message);
    }
}

public class BookingController : BaseController
{
    private readonly IBookingService _bookingService;

    public BookingController(ILogger<BaseController> logger, IBookingService bookingService)
        : base(logger)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBookingsByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return HandleBadRequest("Failed to fetch bookings: Name is required.");
        }

        var result = await _bookingService.GetBookings(name);

        return result.Success ? Ok(result) : HandleBadRequest(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> BookAppointment(BookingRequest request)
    {
        if (!ModelState.IsValid)
        {
            return HandleBadRequest("Invalid booking request model state.");
        }

        var result = await _bookingService.BookAppointmentAsync(request);

        if (result.Success)
        {
            return Ok(result);
        }

        return result.IsConflict ? Conflict(result.Message) : HandleBadRequest(result.Message);
    }
}