using FUEL_DISPATCH_API.Utils.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;

public partial class Dispenser
{
    public int? Id { get; set; }
    [Required] public string? Code { get; set; }
    public string? Status { get; set; } = ValidationConstants.ActiveStatus;
    [Required] public int? BranchIslandId { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public string? UpdatedBy { get; set; }
    public int? BranchOfficeId { get; set; }
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
    [JsonIgnore] public virtual BranchIsland? BranchIsland { get; set; }

    [JsonIgnore] public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = new List<WareHouseMovement>();
}