using Microsoft.Extensions.Caching.Memory;
using SettlementBookingAPI.Models.Entities;
using SettlementBookingAPI.Repositories.Interfaces;
using System.Diagnostics;

namespace SettlementBookingAPI.Repositories
{
    // This proxy class acts as an intermediary between the BookingService and the BookingRepository,
    // providing an additional layer of abstraction for interacting with the data access layer.
    // It helps decouple the service layer from the specific implementation details of the repository,
    // allowing for easier testing, maintainability, and potential future changes in the data access logic.
    public class BookingRepositoryProxy : IBookingRepositoryProxy
    {
        private readonly IBookingRepository _repository;
        private readonly ILogger<BookingRepositoryProxy> _logger;
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private const int MaxRetryAttempts = 3;

        public BookingRepositoryProxy(IBookingRepository repository, ILogger<BookingRepositoryProxy> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<BookingEntity>> GetBookingsAtTimeAsync(DateTime bookingTime)
        {
            if (_cache.TryGetValue("BookingsAtTime_" + bookingTime.ToString(), out IEnumerable<BookingEntity> cachedBookings))
            {
                _logger.LogInformation("Returning bookings from cache.");
                return cachedBookings;
            }

            var bookings = await _repository.GetBookingsAtTimeAsync(bookingTime);
            _cache.Set("BookingsAtTime_" + bookingTime.ToString(), bookings, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
            return bookings;
        }

        public Task<IEnumerable<BookingEntity>> GetBookingsByNameAsync(string name)
        {
            int retryAttempt = 0;
            do
            {
                try
                {
                    return _repository.GetBookingsByNameAsync(name);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while fetching bookings: {ex.Message}. Retrying...");
                }
            } while (++retryAttempt <= MaxRetryAttempts);

            throw new Exception("Failed to fetch bookings after multiple retry attempts.");
        }

        public Task AddBookingAsync(BookingEntity booking)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {

                return _repository.AddBookingAsync(booking);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"GetBookingsByNameAsync took: {stopwatch.ElapsedMilliseconds}ms");
            }
        }

    }
}