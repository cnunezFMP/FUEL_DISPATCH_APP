using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Vehicle
{
    public int Id { get; set; }
    public string? Ficha { get; set; }
    [Required] public int? MakeId { get; set; }
    [Required] public int? ModelId { get; set; }
    [Required] public int? GenerationId { get; set; }
    // DONE: Agregar propiedad VIN. 
    [Required] public string? VIN { get; set; }
    [Required] public int? ModEngineId { get; set; }
    public int? DriverId { get; set; }
    public string? Status { get; set; }

    public string? CreatedBy { get; set; }
    [Required] public int CompanyId { get; set; }
    [Required] public int BranchOfficeId { get; set; }
    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [Required] public decimal? AverageConsumption { get; set; }

    [Required] public string? Color { get; set; }

    [Required] public decimal? FuelTankCapacity { get; set; }
    public decimal? Odometer { get; set; }
    [Required]
    public int? OdometerMeasureId { get; set; }
    [Required] public string? Plate { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = new List<WareHouseMovement>();
    public virtual Driver? Driver { get; set; }

    public virtual Generation? Generation { get; set; }

    public virtual Make? Make { get; set; }

    public virtual OdometerMeasure? Measure { get; set; }

    public virtual ModEngine? ModEngine { get; set; }

    public virtual Model? Model { get; set; }

    [JsonIgnore] public virtual Companies? Company { get; set; }
    [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovementRequest> Requests { get; set; } = new List<WareHouseMovementRequest>();

    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}