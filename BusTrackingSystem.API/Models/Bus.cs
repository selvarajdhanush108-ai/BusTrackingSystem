
namespace BusTrackingSystem.API.Models;

public class Bus
{
    public int BusId { get; set; }

    public string BusNumber { get; set; } = string.Empty;

    public string RouteNumber { get; set; } = string.Empty;

    public string? DriverName { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Location> Locations { get; set; }
        = new List<Location>();
}
