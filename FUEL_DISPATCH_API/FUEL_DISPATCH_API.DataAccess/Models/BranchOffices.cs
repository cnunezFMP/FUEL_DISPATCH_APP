using FUEL_DISPATCH_API.Utils.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;

public partial class BranchOffices
{
    [JsonIgnore] public virtual ICollection<BranchIsland>? BranchIslands { get; set; } = new List<BranchIsland>();
    [Required] public string? Code { get; set; }
    [JsonIgnore] public virtual Companies? Company { get; set; }

    public int? CompanyId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }
    [Required] public string? Email { get; set; }
    public string? Email2 { get; set; }
    [Required] public string? FullLocation { get; set; }
    public int Id { get; set; }
    [Required] public string? Name { get; set; }
    [Phone, Required] public string? Phone { get; set; }
    [Phone] public string? Phone2 { get; set; }

    [Required] public string? Representative { get; set; }
    public string? Status { get; set; } = ValidationConstants.ActiveStatus;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    [JsonIgnore] public virtual ICollection<UsersBranchOffices> UsersBranchOffices { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Dispenser> Dispensers { get; set; } = [] ;
    [JsonIgnore] public virtual ICollection<Vehicle> Vehicles { get; set; } = [];
    [JsonIgnore] public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = [];
    [JsonIgnore] public virtual ICollection<WareHouse> WareHouses { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Driver> Drivers { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Booking>? Bookings { get; set; } = [];
    [JsonIgnore] public virtual ICollection<User> Users { get; set; } = [];
    [JsonIgnore] public virtual ICollection<EmployeeConsumptionLimits> EmployeeConsumptionLimits{ get; set; } = [];
    [JsonIgnore] public virtual ICollection<WareHouseMovementRequest> WareHouseMovementRequests { get; set; } = [];
    [JsonIgnore] public virtual ICollection<UsersRols> UserRols { get; set; } = [];

}