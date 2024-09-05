using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Role
{
    public int? Id { get; set; }
    public string? RolName { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore] public virtual ICollection<UsersRols> UsersRols { get; set; } = [];
    [JsonIgnore] public virtual ICollection<User>? Users { get; set; }
}