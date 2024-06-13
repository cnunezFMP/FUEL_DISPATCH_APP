using FUEL_DISPATCH_API.Utils.Constants;
using System;
using System.Collections.Generic;

namespace FUEL_DISPATCH_API.DataAccess.Models;

public partial class Dispenser
{
    public int Id { get; set; }

    public string Dispenser_Name { get; set; }
    public int? Hoses { get; set; }
    public string Status { get; set; } = ValidationConstants.ActiveStatus;  
    public int BranchIslandId { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    public virtual BranchIsland BranchIsland { get; set; }
    public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = new List<WareHouseMovement>();
}