using FlightInformationAPI.Models;

namespace FlightInformationAPI.Interfaces
{
    public interface IFlightCsvLoader
    {
        IEnumerable<Flight> Load(string path);
    }
}
