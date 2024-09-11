using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Make
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ImgUrl { get; set; }

    public string? Url { get; set; }

    public virtual ICollection<Model> Models { get; set; } = [];

    [JsonIgnore] public virtual ICollection<Vehicle> Vehicles { get; set; } = [];
}