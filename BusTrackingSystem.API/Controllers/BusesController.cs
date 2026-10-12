
using BusTrackingSystem.API.Data;
using BusTrackingSystem.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusTrackingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusesController : ControllerBase
{
    private readonly BusTrackingDbContext _context;

    public BusesController(BusTrackingDbContext context)
    {
        _context = context;
    }

    // GET: api/buses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bus>>> GetBuses()
    {
        var buses = await _context.Buses
            .AsNoTracking()
            .ToListAsync();

        return Ok(buses);
    }

    // GET: api/buses/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Bus>> GetBus(int id)
    {
        var bus = await _context.Buses
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BusId == id);

        if (bus == null)
            return NotFound(new { message = "Bus not found." });

        return Ok(bus);
    }

    // POST: api/buses
    [HttpPost]
    public async Task<ActionResult<Bus>> CreateBus(
        CreateBusRequest request)
    {
        var busNumberExists = await _context.Buses
            .AnyAsync(b => b.BusNumber == request.BusNumber);

        if (busNumberExists)
            return Conflict(new { message = "Bus number already exists." });

        var bus = new Bus
        {
            BusNumber = request.BusNumber,
            RouteNumber = request.RouteNumber,
            DriverName = request.DriverName,
            IsActive = true
        };

        _context.Buses.Add(bus);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetBus),
            new { id = bus.BusId },
            bus);
    }

    // PUT: api/buses/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBus(
        int id,
        UpdateBusRequest request)
    {
        var bus = await _context.Buses.FindAsync(id);

        if (bus == null)
            return NotFound(new { message = "Bus not found." });

        var duplicateNumber = await _context.Buses.AnyAsync(
            b => b.BusNumber == request.BusNumber && b.BusId != id);

        if (duplicateNumber)
            return Conflict(new { message = "Bus number already exists." });

        bus.BusNumber = request.BusNumber;
        bus.RouteNumber = request.RouteNumber;
        bus.DriverName = request.DriverName;
        bus.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/buses/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBus(int id)
    {
        var bus = await _context.Buses.FindAsync(id);

        if (bus == null)
            return NotFound(new { message = "Bus not found." });

        _context.Buses.Remove(bus);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public record CreateBusRequest(
    string BusNumber,
    string RouteNumber,
    string? DriverName);

public record UpdateBusRequest(
    string BusNumber,
    string RouteNumber,
    string? DriverName,
    bool IsActive);
