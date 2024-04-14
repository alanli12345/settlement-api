namespace SettlementBookingAPI.Helpers.Interfaces
{
    public interface IBookingHelper
    {
        bool IsValidBookingTime(DateTime bookingTime);
        DateTime? ParseBookingTime(string bookingTime);
    }
}
