using FUEL_DISPATCH_API.Utils.Constants;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Road
{
    public int Id { get; set; }

    public string APoint { get; set; }

    public string BPoint { get; set; }
    public string? CPoint { get; set; } 
    public string? DPoint { get; set; }
    public string? EPoint { get; set; }
    public string? FPoint { get; set; }

    public string? Code { get; set; }

    public TimeOnly? StimatedTime { get; set; }

    public string? Status { get; set; } = ValidationConstants.ActiveStatus;

    public int? ZoneId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = new List<WareHouseMovement>();
    [JsonIgnore]
    public virtual Zone? Zone { get; set; }
}