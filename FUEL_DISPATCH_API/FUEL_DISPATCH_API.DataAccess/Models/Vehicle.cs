using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using FUEL_DISPATCH_API.DataAccess.Enums;
namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class Vehicle
{
    public int Id { get; set; }
    public string? Ficha { get; set; }
    [Required] public int? MakeId { get; set; }
    [Required] public int? ModelId { get; set; }
    public int? GenerationId { get; set; }
    // DONE: Agregar propiedad VIN.
    [RegularExpression("^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "Ingrese un numero de Chasis valido.")] public string? VIN { get; set; }
    public int? ModEngineId { get; set; }
    [Required] public int Year { get; set; }
    public int? DriverId { get; set; }
    public VehicleStatussesEnum? Status { get; set; } = VehicleStatussesEnum.Active;
    public string? CreatedBy { get; set; }
    public int? CompanyId { get; set; }
    public int? BranchOfficeId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [Required] public decimal AverageConsumption { get; set; }
    [Required] public string? Color { get; set; }
    [Required] public decimal FuelTankCapacity { get; set; }
    public decimal? Odometer { get; set; }
    [Required] public int OdometerMeasureId { get; set; }
    [Required, RegularExpression("^[A-Z0-9]{1,2}[-]?[A-Z0-9]{3,5}$", ErrorMessage = "Ingrese un numero de placa valido.")]
    public string Plate { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = [];
    public virtual Driver? Driver { get; set; }
    [JsonIgnore] public virtual Generation? Generation { get; set; }
    [JsonIgnore] public virtual Make? Make { get; set; }
    [JsonIgnore] public virtual OdometerMeasure? Measure { get; set; }
    [JsonIgnore] public virtual ModEngine? ModEngine { get; set; }
    [JsonIgnore] public virtual Model? Model { get; set; }
    [JsonIgnore] public virtual Companies? Company { get; set; }
    [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
    [JsonIgnore] public virtual ICollection<WareHouseMovementRequest> Requests { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = [];
    [JsonIgnore] public virtual ICollection<Maintenance> Maintenances { get; set; } = [];
}