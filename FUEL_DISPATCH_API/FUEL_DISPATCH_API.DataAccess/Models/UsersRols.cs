using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;

public partial class UsersRols
{
    [Required] public int UserId { get; set; }
    [Required] public int RolId { get; set; }
    public int? CompanyId { get; set; }
    public int? BranchOfficeId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    [JsonIgnore] public virtual Role? Rol { get; set; }

    [JsonIgnore] public virtual User? User { get; set; }
    [JsonIgnore] public virtual Companies? Company { get; set; }
    [JsonIgnore] public virtual BranchOffices? BranchOffice{ get; set; }

}