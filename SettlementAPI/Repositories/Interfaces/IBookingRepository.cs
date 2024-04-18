﻿using SettlementBookingAPI.Models.Entities;

namespace SettlementBookingAPI.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<BookingEntity>> GetBookingsByNameAsync(string name);
        Task<IEnumerable<BookingEntity>> GetBookingsAtTimeAsync(DateTime bookingTime);
        Task AddBookingAsync(BookingEntity booking);
    }
}
