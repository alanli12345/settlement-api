namespace SettlementBookingAPI.Strategies.Interfaces
{
    public interface IBookingStrategy
    {
        Task<bool> CanBookAsync(string name, DateTime bookingTime);
    }
}
