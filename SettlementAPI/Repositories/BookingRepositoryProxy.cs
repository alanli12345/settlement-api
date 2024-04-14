using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Repositories.Interfaces;

namespace SettlementBookingAPI.Repositories
{
    public class BookingRepositoryProxy : IBookingRepositoryProxy
    {
        private readonly IBookingRepository _repository;

        public BookingRepositoryProxy(IBookingRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Booking>> GetBookingsAtTimeAsync(DateTime bookingTime)
        {
            return _repository.GetBookingsAtTimeAsync(bookingTime);
        }

        public Task<IEnumerable<Booking>> GetBookingsByNameAsync(string name)
        {
            return _repository.GetBookingsByNameAsync(name);
        }

        public Task AddBookingAsync(Booking booking)
        {
            return _repository.AddBookingAsync(booking);
        }
    }
}
