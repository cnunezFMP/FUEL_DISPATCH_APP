using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FUEL_DISPATCH_API.Utils.Constants;
namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Driver
{
    public int Id { get; set; }
    public string? Email { get; set; }
    [Required] public string? FullName { get; set; }
    public int? CompanyId { get; set; }
    [Required] public int? BranchOfficeId { get; set; }
    [Required, Phone] public string? PhoneNumber { get; set; }
    // DONE: Hacer que la fecha no pueda ser menor de edad ni que sea la del mismo dia en un validador de FluentValidation.
    [Required] public DateTime? BirthDate { get; set; }
    [Required] public string? FullDirection { get; set; }
    [Required] public DateTime? LicenceExpDate { get; set; }
    public string? Status { get; set; } = ValidationConstants.ActiveStatus;
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [Required, MinLength(13), MaxLength(13)] public string? Identification { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovement>? WareHouseMovements { get; set; } = [];
    [JsonIgnore] public virtual ICollection<User>? User { get; set; } = new List<User>();
    [JsonIgnore] public virtual ICollection<Vehicle>? Vehicles { get; set; } = new List<Vehicle>();
    [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovementRequest>? Requests { get; set; } = new List<WareHouseMovementRequest>();
    [JsonIgnore] public virtual ICollection<EmployeeConsumptionLimits>? EmployeeConsumptionLimits { get; set; } = new List<EmployeeConsumptionLimits>();
    [JsonIgnore] public virtual ICollection<DriverMethodOfComsuption> DriverMethodsOfComsuption { get; set; } = new List<DriverMethodOfComsuption>();
    [JsonIgnore] public virtual Companies? Company { get; set; }
    [JsonIgnore] public virtual ICollection<Booking>? Bookings { get; set; } = new List<Booking>();

}