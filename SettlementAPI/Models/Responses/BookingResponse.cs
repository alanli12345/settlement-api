namespace SettlementBookingAPI.Models.Responses
{
    public class BookingResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Guid BookingId { get; set; }
        public bool IsConflict { get; set; }

        public BookingResponse(bool success, string? message, Guid bookingId, bool isConflict = false)
        {
            Success = success;
            Message = message;
            BookingId = bookingId;
            IsConflict = isConflict;
        }
    }
}
