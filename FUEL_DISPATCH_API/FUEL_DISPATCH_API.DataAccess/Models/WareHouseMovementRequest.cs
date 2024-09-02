using FUEL_DISPATCH_API.DataAccess.Enums;
using FUEL_DISPATCH_API.Utils.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class WareHouseMovementRequest
    {

        public int? Id { get; set; } // Primary Key with IDENTITY
        [Required] public int? WareHouseId { get; set; }
        public int? ToWareHouseId { get; set; }
        public int? DriverId { get; set; }
        [Required] public MovementsTypesEnum? Type { get; set; }   // Salida o Transferencia.
        public int? VehicleId { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchOfficeId { get; set; }
        public string? Status { get; set; } = ValidationConstants.PendingStatus;
        [Required] public decimal Qty { get; set; } // Quantity (decimal for precision)
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        [JsonIgnore] public virtual WareHouse? WareHouse { get; set; }
        [JsonIgnore] public virtual WareHouse? ToWareHouse { get; set; }
        [JsonIgnore] public Vehicle? Vehicle { get; set; } // Foreign Key relationship with Vehicle
        [JsonIgnore] public Driver? Driver { get; set; } // Foreign Key relationship with Driver
        [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
        [JsonIgnore] public virtual Companies? Company { get; set; }
    }
}
