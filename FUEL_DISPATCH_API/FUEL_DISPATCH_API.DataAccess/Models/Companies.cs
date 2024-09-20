using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;
/// <summary>
/// Esta es la clase de las compañias
/// </summary>
public partial class Companies
{
    public int Id { get; set; }

    [Required] public string? Name { get; set; }

    [Required] public string? FullAddress { get; set; }

    [Required] public string? PostalCode { get; set; }
    [Phone, Required] public string? PhoneNumber { get; set; }
    [Phone] public string? PhoneNumber2 { get; set; }

    [Required] public string? CompanyRNC { get; set; }

    [Required] public string? EmailAddress { get; set; }
    public string? EmailAddress2 { get; set; }

    public string? Website { get; set; }
    public string? Industry { get; set; }
    public DateTime? DateEstablished { get; set; }
    [Required] public string? CEOFounder { get; set; }
    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovement>? WareHouseMovements { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Vehicle>? Vehicles { get; set; } = [];
    [JsonIgnore] public virtual ICollection<ArticleDataMaster>? Articles { get; set; } = [];
    [JsonIgnore] public virtual ICollection<User>? Users { get; set; } = [];
    [JsonIgnore] public virtual ICollection<WareHouse>? WareHouses { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Driver> Drivers { get; set; } = [];
    [JsonIgnore] public virtual ICollection<BranchOffices>? BranchOffices { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Part>? Parts { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Booking>? Bookings { get; set; } = [];
    [JsonIgnore] public virtual ICollection<BranchIsland>? BranchIslands { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Dispenser>? Dispensers { get; set; } = [];
    public CompanySAPParams? CompanySAPParams { get; set; }
    [JsonIgnore] public virtual ICollection<EmployeeConsumptionLimits> EmployeeConsumptionLimits { get; set; } = [];
    [JsonIgnore] public virtual ICollection<UsersBranchOffices> UsersBranches { get; set; } = [];
    [JsonIgnore] public virtual ICollection<WareHouseMovementRequest> WareHouseMovementRequests { get; set; } = [];
    [JsonIgnore] public virtual ICollection<UsersRols> UsersRols { get; set; } = [];
}
