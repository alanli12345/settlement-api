using SettlementBookingAPI.Models.Entities;
using SettlementBookingAPI.Repositories.Interfaces;
using System.Transactions;

namespace SettlementBookingAPI.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly List<BookingEntity> _bookings;

        public BookingRepository(List<BookingEntity> bookings)
        {
            _bookings = bookings;
        }

        // Made the following functions async to simulate async db calls
        public async Task<IEnumerable<BookingEntity>> GetBookingsByNameAsync(string name)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var userBookings = _bookings.Where(b =>
                    b.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                );

                return await Task.FromResult(userBookings);
            }
        }

        public async Task<IEnumerable<BookingEntity>> GetBookingsAtTimeAsync(DateTime bookingTime)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
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
        }

        public Task AddBookingAsync(BookingEntity booking)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _bookings.Add(booking);
                return Task.CompletedTask;
            }
        }
    }
}
