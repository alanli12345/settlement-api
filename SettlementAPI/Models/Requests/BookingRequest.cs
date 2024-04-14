using System.ComponentModel.DataAnnotations;

namespace SettlementBookingAPI.Models.Requests
{
    public class BookingRequest
    {
        [Required(ErrorMessage = "Booking time is required.")]
        public string BookingTime { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }

    public class Booking
    {
        public Guid BookingId { get; set; }
        public DateTime BookingTime { get; set; }
        public string Name { get; set; }
    }
}
