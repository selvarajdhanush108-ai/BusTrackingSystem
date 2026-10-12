
using BusTrackingSystem.API.Data;
using BusTrackingSystem.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusTrackingSystem.API.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    private readonly BusTrackingDbContext _context;

    public LocationsController(BusTrackingDbContext context)
    {
        _context = context;
    }

    // POST: api/locations
    [HttpPost]
    public async Task<IActionResult> AddLocation(
        CreateLocationRequest request)
    {
        var bus = await _context.Buses.FindAsync(request.BusId);

        if (bus == null)
        {
            return NotFound(new
            {
                message = "Bus not found."
            });
        }

        if (request.Latitude is < -90 or > 90 ||
            request.Longitude is < -180 or > 180)
        {
            return BadRequest(new
            {
                message = "Invalid latitude or longitude."
            });
        }

        var location = new Location
        {
            BusId = request.BusId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            RecordedAt = DateTime.UtcNow
        };

       
_context.Locations.Add(location);
await _context.SaveChangesAsync();

return CreatedAtAction(
    nameof(GetLatestLocation),
    new { busId = location.BusId },
    new
    {
        location.LocationId,
        location.BusId,
        location.Latitude,
        location.Longitude,
        location.RecordedAt
    });

    }

    // GET: api/locations/bus/1
    [HttpGet("bus/{busId:int}")]
    public async Task<ActionResult<Location>> GetLatestLocation(
        int busId)
    {
        var busExists = await _context.Buses
            .AnyAsync(b => b.BusId == busId);

        if (!busExists)
        {
            return NotFound(new
            {
                message = "Bus not found."
            });
        }

        var location = await _context.Locations
            .AsNoTracking()
            .Where(l => l.BusId == busId)
            .OrderByDescending(l => l.RecordedAt)
            .ThenByDescending(l => l.LocationId)
            .FirstOrDefaultAsync();

        if (location == null)
        {
            return NotFound(new
            {
                message = "No location has been recorded for this bus."
            });
        }

        return Ok(location);
    }
}

public record CreateLocationRequest(
    int BusId,
    double Latitude,
    double Longitude);
