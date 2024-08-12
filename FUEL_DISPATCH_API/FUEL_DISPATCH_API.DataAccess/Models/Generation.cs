using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;

public partial class Generation
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? ImgUrl { get; set; }
    public string? Url { get; set; }
    public int? ModelId { get; set; }
    public virtual ICollection<ModEngine> ModEngines { get; set; } = new List<ModEngine>();
    public virtual Model? Model { get; set; }
    [JsonIgnore] public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}