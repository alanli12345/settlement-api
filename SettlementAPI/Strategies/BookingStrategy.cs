using SettlementBookingAPI.Repositories.Interfaces;
using SettlementBookingAPI.Strategies.Interfaces;

namespace SettlementBookingAPI.Strategies
{
    public class BookingStrategy : IBookingStrategy
    {
        private readonly IBookingRepositoryProxy _bookingRepositoryProxy;

        public BookingStrategy(IBookingRepositoryProxy bookingRepositoryProxy)
        {
            _bookingRepositoryProxy = bookingRepositoryProxy;
        }

        public async Task<bool> CanBookAsync(string name, DateTime bookingTime)
        {
            var bookingsAtTime = await _bookingRepositoryProxy.GetBookingsAtTimeAsync(bookingTime);

            if (bookingsAtTime.Count() >= 4)
            {
                return false; 
            }

            var userBookings = await _bookingRepositoryProxy.GetBookingsByNameAsync(name);

            foreach (var booking in userBookings)
            {
                if (bookingTime >= booking.BookingTime && bookingTime < booking.BookingTime.AddHours(1))
                {
                    return false; 
                }
            }

            return true;
        }
    }
}
