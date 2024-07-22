using FUEL_DISPATCH_API.Utils.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Road
{
    public int Id { get; set; }

    [Required] public string APoint { get; set; }

    [Required] public string BPoint { get; set; }
    public string? CPoint { get; set; }
    public string? DPoint { get; set; }
    public string? EPoint { get; set; }
    public string? FPoint { get; set; }

    [Required] public string Code { get; set; }

    public TimeOnly? StimatedTime { get; set; }
    [Required] public int CompanyId { get; set; }
    public string? Status { get; set; } = ValidationConstants.ActiveStatus;

    [Required] public int ZoneId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    [JsonIgnore] public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = new List<WareHouseMovement>();

    [JsonIgnore] public virtual Zone? Zone { get; set; }
    [JsonIgnore] public virtual Companies? Company { get; set; }
}