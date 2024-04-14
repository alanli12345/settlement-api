using SettlementBookingAPI.Services.Interfaces;
using SettlementBookingAPI.Services;
using SettlementBookingAPI.Repositories.Interfaces;
using SettlementBookingAPI.Repositories;
using SettlementBookingAPI.Strategies.Interfaces;
using SettlementBookingAPI.Strategies;
using SettlementBookingAPI.Helpers.Interfaces;
using SettlementBookingAPI.Helpers;
using System.Collections.Concurrent;
using SettlementBookingAPI.Models.Requests;

namespace SettlementBookingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSingleton<List<Booking>>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingRepositoryProxy, BookingRepositoryProxy>();
            builder.Services.AddScoped<IBookingHelper, BookingHelper>();
            builder.Services.AddScoped<IBookingStrategy, BookingStrategy>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}