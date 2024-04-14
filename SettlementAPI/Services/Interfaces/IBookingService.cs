using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Models.Responses;

namespace SettlementBookingAPI.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> BookAppointmentAsync(BookingRequest request);
        Task<BookingsListResponse> GetBookings(string name);
    }
}
