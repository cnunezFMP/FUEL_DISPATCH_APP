using FUEL_DISPATCH_API.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Driver
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Email { get; set; }
    [Required] public string? FullName { get; set; }
    public int? CompanyId { get; set; }
    public int? BranchOfficeId { get; set; }
    [Required, Phone] public string? PhoneNumber { get; set; }
    [Required] public DateTime? BirthDate { get; set; }
    [Required] public string? FullDirection { get; set; }
    [Required] public DateTime? LicenceExpDate { get; set; }
    public ActiveInactiveStatussesEnum? Status { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [Required, MinLength(11), MaxLength(11)] public string? Identification { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovement>? WareHouseMovements { get; set; } = [];
    [JsonIgnore] public virtual ICollection<User>? User { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Vehicle>? Vehicles { get; set; } = [];
    [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovementRequest>? Requests { get; set; } = [];
    [JsonIgnore] public virtual ICollection<EmployeeConsumptionLimits>? EmployeeConsumptionLimits { get; set; } = [];
    [JsonIgnore] public virtual ICollection<DriverMethodOfComsuption> DriverMethodsOfComsuption { get; set; } = [];
    [JsonIgnore] public virtual Companies? Company { get; set; }
    [JsonIgnore] public virtual ICollection<Booking>? Bookings { get; set; } = [];

}