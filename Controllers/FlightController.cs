using FlightInformationAPI.DTOs;
using FlightInformationAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightService _flightService;
    private readonly ILogger<FlightsController> _logger;

    public FlightsController(IFlightService flightService, ILogger<FlightsController> logger)
    {
        _flightService = flightService;
        _logger = logger;
    }

    // GET: api/flights  
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FlightDto>>> GetAll(
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10)
    {
        var pagedFlights = await _flightService.GetAllAsync(pageNumber, pageSize);
        return Ok(pagedFlights);
    }

    // GET: api/flights/{id}  
    [HttpGet("{id:int}")]
    public async Task<ActionResult<FlightDto>> GetById(int id)
    {
        var flight = await _flightService.GetByIdAsync(id);
        if (flight == null)
        {
            _logger.LogWarning("Flight with id {Id} not found.", id);
            return NotFound();
        }
        return Ok(flight);
    }

    // POST: api/flights  
    [HttpPost]
    public async Task<ActionResult<FlightDto>> Create([FromBody] FlightCreateDto createDto)
    {
        var flight = await _flightService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = flight.Id }, flight);
    }

    // PUT: api/flights/{id}  
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] FlightUpdateDto updateDto)
    {
        var exists = await _flightService.ExistsAsync(id);
        if (!exists)
        {
            _logger.LogWarning("Flight with id {Id} not found.", id);
            return NotFound();
        }

        await _flightService.UpdateAsync(id, updateDto);
        return NoContent();
    }

    // DELETE: api/flights/{id}  
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var exists = await _flightService.ExistsAsync(id);
        if (!exists)
            return NotFound();

        await _flightService.DeleteAsync(id);

        return NoContent();
    }

    // GET: api/flights/search?airline=xxx&departureAirport=yyy&fromDate=2025-01-01&toDate=2025-02-01  
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FlightDto>>> Search(
        [FromQuery] string? airline,
        [FromQuery] string? departureAirport,
        [FromQuery] string? arrivalAirport,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var results = await _flightService.SearchAsync(airline, departureAirport, arrivalAirport, fromDate, toDate, pageNumber, pageSize);
        return Ok(results);
    }
}
