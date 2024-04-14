using SettlementBookingAPI.Models.Requests;

namespace SettlementBookingAPI.Models.Responses
{
    public class BookingsListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Booking> Bookings { get; set; }

        public BookingsListResponse(bool success, string message,  List<Booking> bookings)
        {
            Success = success;
            Message = message;
            Bookings = bookings;
        }
    }
}
