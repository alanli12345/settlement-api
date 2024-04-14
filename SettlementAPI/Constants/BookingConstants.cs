namespace SettlementBookingAPI.Constants
{
    public static class BookingConstants
    {
        public const string EmptyNameMessage = "Name must not be empty";
        public const string InvalidFormatMessage = "Invalid booking time format. Please use the HH:mm format.";
        public const string OutOfHoursMessage = "Booking time is out of business hours.";
        public const string UnavailableMessage = "Booking time selected is not available";
        public const string SuccessfulMessage = "Booking successful.";
        public const string RetrievedMessage = "Bookings retrieved.";
        public const string InvalidModelMessage = "Invalid booking request model state.";
    }
}
