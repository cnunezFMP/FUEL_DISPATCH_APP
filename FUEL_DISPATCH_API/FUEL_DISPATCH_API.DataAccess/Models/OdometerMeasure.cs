using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class OdometerMeasure
{
    public int Id { get; set; }

    public string? Measurename { get; set; }

    [JsonIgnore] public virtual ICollection<Vehicle> Vehicle { get; set; } = new List<Vehicle>();
}