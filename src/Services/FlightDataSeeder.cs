using FlightInformationAPI.DBContext;
using FlightInformationAPI.Interfaces;

namespace FlightInformationAPI.Services
{
    public class FlightDataSeeder
    {
        private readonly FlightDbContext _context;
        private readonly IFlightCsvLoader _csvLoader;

        public FlightDataSeeder(FlightDbContext context, IFlightCsvLoader csvLoader)
        {
            _context = context;
            _csvLoader = csvLoader;
        }

        public void Seed(string csvPath)
        {
            var flights = _csvLoader.Load(csvPath);
            _context.Flights.AddRange(flights);
            _context.SaveChanges();
        }
    }
}
