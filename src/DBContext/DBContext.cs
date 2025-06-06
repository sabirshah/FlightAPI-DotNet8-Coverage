using FlightInformationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightInformationAPI.DBContext
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options) { }

        public DbSet<Flight> Flights => Set<Flight>();
    }
}
