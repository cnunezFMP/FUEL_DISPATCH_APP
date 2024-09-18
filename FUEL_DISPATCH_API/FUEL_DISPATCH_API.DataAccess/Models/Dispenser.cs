using FUEL_DISPATCH_API.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Dispenser
{
    public int Id { get; set; }
    [Required] public string? Code { get; set; }
    public ActiveInactiveStatussesEnum? Status { get; set; } = ActiveInactiveStatussesEnum.Active;
    public int? BranchIslandId { get; set; }
    public int? CompanyId { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public int? BranchOfficeId { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
    [JsonIgnore] public virtual Companies? Company { get; set; }
    public virtual BranchIsland? BranchIsland { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = [];
}