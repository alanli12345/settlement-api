using AutoMapper;
using SettlementBookingAPI.Helpers.Interfaces;
using SettlementBookingAPI.Models.Entities;
using SettlementBookingAPI.Models.Requests;
using SettlementBookingAPI.Models.Responses;
using SettlementBookingAPI.Repositories.Interfaces;
using SettlementBookingAPI.Services.Interfaces;
using SettlementBookingAPI.Strategies.Interfaces;
using static SettlementBookingAPI.Constants.BookingConstants;
using static SettlementBookingAPI.Models.Responses.BookingsListResponse;

namespace SettlementBookingAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepositoryProxy _bookingRepositoryProxy;
        private readonly IBookingStrategy _bookingStrategy;
        private readonly IBookingHelper _bookingHelper;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepositoryProxy bookingRepositoryProxy, IBookingStrategy bookingStrategy, IBookingHelper bookingHelper, IMapper mapper)
        {
            _bookingRepositoryProxy = bookingRepositoryProxy;
            _bookingStrategy = bookingStrategy;
            _bookingHelper = bookingHelper;
            _mapper = mapper;
        }

        public async Task<BookingResponse> BookAppointmentAsync(BookingRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return new BookingResponse(false, EmptyNameMessage, Guid.Empty);

                DateTime? bookingDateTime = _bookingHelper.ParseBookingTime(request.BookingTime);

                if (!bookingDateTime.HasValue)
                    return new BookingResponse(false, InvalidFormatMessage, Guid.Empty);

                if (!_bookingHelper.IsValidBookingTime(bookingDateTime.Value))
                    return new BookingResponse(false, OutOfHoursMessage, Guid.Empty);

                if (!await _bookingStrategy.CanBookAsync(request.Name, bookingDateTime.Value))
                    return new BookingResponse(false, UnavailableMessage, Guid.Empty, true);

                var bookingId = Guid.NewGuid();
                var booking = new BookingEntity { BookingId = bookingId, BookingTime = bookingDateTime.Value, Name = request.Name };

                await _bookingRepositoryProxy.AddBookingAsync(booking);
                return new BookingResponse(true, SuccessfulMessage, bookingId);
            }
            catch (Exception ex)
            {
                return new BookingResponse(false, ex.Message, Guid.Empty);
            }
        }

        public async Task<BookingsListResponse> GetBookings(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return new BookingsListResponse(false, EmptyNameMessage, new List<Booking>());

                var result = await _bookingRepositoryProxy.GetBookingsByNameAsync(name);
                var bookings = _mapper.Map<IEnumerable<Booking>>(result);

                if (!result.Any())
                    return new BookingsListResponse(false, "No bookings found for the specified name.", new List<Booking>());

                return new BookingsListResponse(true, RetrievedMessage, bookings.ToList());
            }
            catch (Exception ex)
            {
                return new BookingsListResponse(false, ex.Message, new List<Booking>());
            }
        }
    }
}
