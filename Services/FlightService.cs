using AutoMapper;
using FlightInformationAPI.DBContext;
using FlightInformationAPI.DTOs;
using FlightInformationAPI.Enumerations;
using FlightInformationAPI.Interfaces;
using FlightInformationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightInformationAPI.Services
{

    public class FlightService : IFlightService
    {
        private readonly FlightDbContext _context;
        private readonly IMapper _mapper;

        public FlightService(FlightDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Flights.AsQueryable();

            var pagedFlights = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FlightDto>>(pagedFlights);
        }

        public async Task<FlightDto?> GetByIdAsync(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            return flight is null ? null : _mapper.Map<FlightDto>(flight);
        }

        public async Task<FlightDto> CreateAsync(FlightCreateDto createDto)
        {
            var flight = _mapper.Map<Flight>(createDto);
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return _mapper.Map<FlightDto>(flight);
        }

        public async Task UpdateAsync(int id, FlightUpdateDto updateDto)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null) return;

            _mapper.Map(updateDto, flight);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Flights.AnyAsync(f => f.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight is null)
                throw new KeyNotFoundException($"Flight with ID {id} not found.");


            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FlightDto>> SearchAsync(
                  string? airline,
                  string? departureAirport,
                  string? arrivalAirport,
                  DateTime? fromDate,
                  DateTime? toDate)
        {
            var query = _context.Flights.AsQueryable();

            if (!string.IsNullOrWhiteSpace(airline))
                query = query.Where(f => f.Airline.Contains(airline));

            if (!string.IsNullOrWhiteSpace(departureAirport))
                query = query.Where(f => f.DepartureAirport.Contains(departureAirport));

            if (!string.IsNullOrWhiteSpace(arrivalAirport))
                query = query.Where(f => f.ArrivalAirport.Contains(arrivalAirport));

            if (fromDate.HasValue)
                query = query.Where(f => f.DepartureTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(f => f.ArrivalTime <= toDate.Value);

            var flights = await query.ToListAsync();
            return _mapper.Map<IEnumerable<FlightDto>>(flights);
        }

    }

}
