using FUEL_DISPATCH_API.Utils.Constants;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;

public partial class Dispenser
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Status { get; set; } = ValidationConstants.ActiveStatus;
    public int BranchIslandId { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    [JsonIgnore]
    public virtual BranchIsland? BranchIsland { get; set; }
    [JsonIgnore]
    public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = new List<WareHouseMovement>();
}