using FlightInformationAPI.DTOs;

namespace FlightInformationAPI.Interfaces
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<FlightDto?> GetByIdAsync(int id);
        Task<FlightDto> CreateAsync(FlightCreateDto createDto);
        Task<bool> ExistsAsync(int id);
        Task UpdateAsync(int id, FlightUpdateDto updateDto);
        Task DeleteAsync(int id);

        Task<IEnumerable<FlightDto>> SearchAsync(
            string? airline,
            string? departureAirport,
            string? arrivalAirport,
            DateTime? fromDate,
            DateTime? toDate);
    }
}
