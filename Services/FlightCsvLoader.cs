using FlightInformationAPI.Enumerations;
using FlightInformationAPI.Interfaces;
using FlightInformationAPI.Models;

namespace FlightInformationAPI.Services
{
    public class FlightCsvLoader : IFlightCsvLoader
    {
        public IEnumerable<Flight> Load(string path)
        {
            var lines = File.ReadAllLines(path).Skip(1); // Skip header
            var flights = new List<Flight>();

            foreach (var line in lines)
            {
                var fields = line.Split(',');

                var flight = new Flight
                {
                    Id = int.Parse(fields[0]),
                    FlightNumber = fields[1],
                    Airline = fields[2],
                    DepartureAirport = fields[3],
                    ArrivalAirport = fields[4],
                    DepartureTime = DateTime.Parse(fields[5]),
                    ArrivalTime = DateTime.Parse(fields[6]),
                    Status = Enum.Parse<FlightStatus>(fields[7])
                };

                flights.Add(flight);
            }

            return flights;
        }
    }

}
