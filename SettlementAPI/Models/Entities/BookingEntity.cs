namespace SettlementBookingAPI.Models.Entities
{
    public class BookingEntity
    {
        public Guid BookingId { get; set; }
        public DateTime BookingTime { get; set; }
        public string Name { get; set; }
    }
}
