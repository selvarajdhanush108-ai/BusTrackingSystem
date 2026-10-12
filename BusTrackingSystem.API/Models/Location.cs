namespace BusTrackingSystem.API.Models;

public class Location
{
    public int LocationId{get;set;}
    public int BusId{get;set;}
    public double Latitude{get;set;}
    public double Longitude{get;set;}
    public DateTime RecordedAt{get;set;} = DateTime.UtcNow;
    public Bus? Bus{get;set;}
}