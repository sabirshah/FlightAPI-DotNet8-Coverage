using FlightInformationAPI.DBContext;
using FlightInformationAPI.DTOs;
using FlightInformationAPI.Interfaces;
using FlightInformationAPI.Mapper;
using FlightInformationAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Tests
{

    public class FlightServiceIntegrationTests : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly FlightDbContext _context;
        private readonly IFlightService _flightService;

        public FlightServiceIntegrationTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FlightDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<FlightDataSeeder>();
            services.AddScoped<IFlightCsvLoader, FlightCsvLoader>();

            // Add your actual AutoMapper profile here
            services.AddAutoMapper(typeof(FlightMappingProfile));

            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<FlightDbContext>();

            // Seed CSV data
            var seeder = _serviceProvider.GetRequiredService<FlightDataSeeder>();
            var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
            while (currentDir != null && !Directory.Exists(Path.Combine(currentDir.FullName, "FlightAPI-DotNet8-Coverage")))
            {
                currentDir = currentDir.Parent;
            }
            var csvPath = Path.Combine(currentDir.FullName + @"\FlightAPI-DotNet8-Coverage", @"src\Data", "FlightInformation.csv");
            seeder.Seed(csvPath);

            _flightService = _serviceProvider.GetRequiredService<IFlightService>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsResults()
        {
            var result = await _flightService.GetAllAsync(1, 10);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectFlight()
        {
            var flight = await _context.Flights.FirstAsync();
            var dto = await _flightService.GetByIdAsync(flight.Id);

            Assert.NotNull(dto);
            Assert.Equal(flight.FlightNumber, dto.FlightNumber);
        }

        [Fact]
        public async Task CreateAsync_CreatesFlight()
        {
            var createDto = new FlightCreateDto
            {
                FlightNumber = "TEST123",
                Airline = "TestAir",
                DepartureAirport = "LAX",
                ArrivalAirport = "JFK",
                DepartureTime = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow.AddHours(5),
                Status = "Scheduled"
            };

            var result = await _flightService.CreateAsync(createDto);

            Assert.NotNull(result);
            Assert.Equal("TEST123", result.FlightNumber);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesFlight()
        {
            var flight = await _context.Flights.FirstAsync();

            var updateDto = new FlightUpdateDto
            {
                FlightNumber = "UPDATED",
                Airline = flight.Airline,
                DepartureAirport = flight.DepartureAirport,
                ArrivalAirport = flight.ArrivalAirport,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Status = "Cancelled"
            };

            await _flightService.UpdateAsync(flight.Id, updateDto);
            var updated = await _context.Flights.FindAsync(flight.Id);

            Assert.Equal("UPDATED", updated.FlightNumber);
            Assert.Equal("Cancelled", updated.Status.ToString());
        }

        [Fact]
        public async Task UpdateAsync_InvalidId_Throws()
        {
            var dto = new FlightUpdateDto
            {
                FlightNumber = "Invalid",
                Airline = "None",
                DepartureAirport = "AAA",
                ArrivalAirport = "BBB",
                DepartureTime = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow.AddHours(1),
                Status = "Canceled"
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _flightService.UpdateAsync(-999, dto));
        }

        [Fact]
        public async Task ExistsAsync_ReturnsTrueForExisting()
        {
            var flight = await _context.Flights.FirstAsync();
            Assert.True(await _flightService.ExistsAsync(flight.Id));
        }

        [Fact]
        public async Task ExistsAsync_ReturnsFalseForNonExisting()
        {
            Assert.False(await _flightService.ExistsAsync(-123));
        }

        [Fact]
        public async Task DeleteAsync_RemovesFlight()
        {
            var flight = await _context.Flights.FirstAsync();
            await _flightService.DeleteAsync(flight.Id);

            Assert.False(await _context.Flights.AnyAsync(f => f.Id == flight.Id));
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_Throws()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _flightService.DeleteAsync(-1));
        }

        [Fact]
        public async Task SearchAsync_ByAirline_Works()
        {
            var airline = (await _context.Flights.FirstAsync()).Airline;
            var results = await _flightService.SearchAsync(airline, null, null, null, null, 1, 10);

            Assert.All(results, r => Assert.Contains(airline, r.Airline));
        }

        [Fact]
        public async Task SearchAsync_ByDateRange_Works()
        {
            var from = DateTime.UtcNow.AddDays(-1);
            var to = DateTime.UtcNow.AddDays(10);

            var results = await _flightService.SearchAsync(null, null, null, from, to, 1, 10);

            Assert.All(results, r =>
            {
                Assert.True(r.DepartureTime >= from);
                Assert.True(r.ArrivalTime <= to);
            });
        }

        public void Dispose()
        {
            _context?.Dispose();
            _serviceProvider?.Dispose();
        }
    }
}
