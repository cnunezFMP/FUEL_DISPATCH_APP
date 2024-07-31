using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;

public partial class UsersRols
{
    public int UserId { get; set; }

    public int RolId { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; }
    [JsonIgnore] public virtual Role? Rol { get; set; }

    [JsonIgnore] public virtual User? User { get; set; }

}