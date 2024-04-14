using SettlementBookingAPI.Models.Requests;

namespace SettlementBookingAPI.Repositories.Interfaces
{
    public interface IBookingRepositoryProxy
    {
        Task<IEnumerable<Booking>> GetBookingsByNameAsync(string name);
        Task<IEnumerable<Booking>> GetBookingsAtTimeAsync(DateTime bookingTime);
        Task AddBookingAsync(Booking booking);
    }
}
