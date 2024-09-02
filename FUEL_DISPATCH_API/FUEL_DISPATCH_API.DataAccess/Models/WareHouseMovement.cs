using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using FUEL_DISPATCH_API.DataAccess.Enums;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public partial class WareHouseMovement
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? VehicleId { get; set; }
        public int? RoadId { get; set; }
        [Required] public string? Dispatcher { get; set; }
        [Required] public int? ItemId { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchOfficeId { get; set; }
        public int? DispenserId { get; set; }
        [Required] public MovementsTypesEnum Type { get; set; }
        [Required] public decimal Qty { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Odometer { get; set; }
        [Required] public int? WareHouseId { get; set; }
        public int? ToWareHouseId { get; set; }
        public string? Notes { get; set; }
        public int? RequestId { get; set; }
        public int? FuelMethodOfComsuptionId { get; set; }
        public string? CreatedBy { get; set; }
        public int? DriverId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
        [JsonIgnore] public virtual Companies? Company { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
        [JsonIgnore] public virtual Driver? Driver { get; set; }
        [JsonIgnore] public virtual Road? Road { get; set; }
        [JsonIgnore] public virtual WareHouseMovementRequest? Request { get; set; }
        public virtual Dispenser? Dispenser { get; set; }
        public virtual WareHouse? WareHouse { get; set; }
        [JsonIgnore] public virtual WareHouse? ToWareHouse { get; set; }
        public virtual ArticleDataMaster? ArticleDataMaster { get; set; }
    }
}
