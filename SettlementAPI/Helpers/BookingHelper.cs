using SettlementBookingAPI.Helpers.Interfaces;
using System.Globalization;

namespace SettlementBookingAPI.Helpers
{
    public class BookingHelper : IBookingHelper
    {
        public bool IsValidBookingTime(DateTime bookingTime)
        {
            return bookingTime.Hour >= 9 &&
                   (bookingTime.Hour < 16 || (bookingTime.Hour == 16 && bookingTime.Minute == 0));
        }

        public DateTime? ParseBookingTime(string bookingTime)
        {
            if (DateTime.TryParseExact(bookingTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
            {
                return parsedDateTime;
            }
            return null;
        }
    }
}
