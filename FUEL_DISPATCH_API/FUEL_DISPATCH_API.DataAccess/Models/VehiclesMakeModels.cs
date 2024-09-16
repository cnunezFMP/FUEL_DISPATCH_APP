using FUEL_DISPATCH_API.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class VehiclesMakeModels
    {
        public int Id { get; set; }
        public string? Ficha { get; set; }
        [Required]
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? GenerationId { get; set; }
        // DONE: Agregar propiedad VIN.
        public string? VIN { get; set; }
        public string? MakeName { get; set; }
        public string? MakeImage { get; set; }
        public string? ModelName { get; set; }
        public string? ModelImage { get; set; }
        public string? GenerationName { get; set; }
        public string? GenerationImage { get; set; }
        public string? ModEngineName { get; set; }
        public int? ModEngineId { get; set; }
        [Required] public int Year { get; set; }
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
        public VehicleStatussesEnum? Status { get; set; }
        public string? CreatedBy { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchOfficeId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required] public decimal AverageConsumption { get; set; }
        [Required] public string Color { get; set; }
        [Required] public decimal FuelTankCapacity { get; set; }
        public decimal? Odometer { get; set; }
        [Required] public int OdometerMeasureId { get; set; }
        [Required] public string Plate { get; set; }
    }
}
