using AutoMapper;
using SettlementBookingAPI.Models.Entities;
using SettlementBookingAPI.Models.Responses;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookingEntity, Booking>();
        CreateMap<IEnumerable<Booking>, IEnumerable<BookingEntity>>();
        CreateMap<IEnumerable<BookingEntity>, List<Booking>>();
    }
}