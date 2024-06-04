#nullable enable
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Dispatch
{
    public int Id { get; set; }
    [Required]
    public string? VehicleToken { get; set; }
    [Required]
    public int? DriverId { get; set; }
    [Required]
    public int RoadId { get; set; }
    public int? RequestId { get; set; }
    [Required]
    public int? DispenserId { get; set; }
    [Required]
    public decimal? Odometer { get; set; }
    [Required]
    public bool? IsForVehicle { get; set; }
    public string? Notes { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    [Required]
    public decimal? Gallons { get; set; }
    public int? BranchOfficeId { get; set; }
    [JsonIgnore]
    public virtual BranchOffices? BranchOffice { get; set; }
    [JsonIgnore]
    public virtual Dispensers? Dispenser { get; set; }
    [JsonIgnore]
    public virtual Drivers? Driver { get; set; }
    [JsonIgnore]
    public virtual Roads? Road { get; set; }
    [JsonIgnore]
    public virtual Vehicles? VehicleTokenNavigation { get; set; }
}