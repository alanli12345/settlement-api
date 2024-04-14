using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Repositories.Interfaces;

namespace SettlementBookingAPI.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly List<Booking> _bookings;

        public BookingRepository(List<Booking> bookings)
        {
            _bookings = bookings;
        }

        // Made the following functions async to simulate async db calls
        public async Task<IEnumerable<Booking>> GetBookingsByNameAsync(string name)
        {
            var userBookings = _bookings.Where(b =>
                b.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
            );

            return await Task.FromResult(userBookings);
        }

        public async Task<IEnumerable<Booking>> GetBookingsAtTimeAsync(DateTime bookingTime)
        {
            // Calculate the start and end time of the booking slot
            var startTime = bookingTime;
            var endTime = startTime.AddHours(1);

            // Retrieve all bookings that overlap with the specified time slot
            var bookingsAtTime = _bookings.Where(b =>
                b.BookingTime >= startTime &&                       // Booking starts after or at the start of the slot
                b.BookingTime < endTime &&                          // Booking ends before the end of the slot
                b.BookingTime.AddHours(1) <= endTime               // Booking duration is exactly 1 hour
            );

            // Return the filtered list of bookings
            return await Task.FromResult(bookingsAtTime);
        }

        public Task AddBookingAsync(Booking booking)
        {
            _bookings.Add(booking);
            return Task.CompletedTask;
        }
    }
}
