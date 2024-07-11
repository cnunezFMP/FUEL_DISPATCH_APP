using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public partial class WareHouseMovement
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }
        public int? RoadId { get; set; }
        public int ItemId { get; set; }
        public int BranchOfficeId { get; set; }
        public int DispenserId { get; set; }
        public string? Type { get; set; }
        public decimal Qty { get; set; }
        public decimal Amount { get; set; }
        public decimal? Odometer { get; set; }
        public int WareHouseId { get; set; }
        public int? ToWareHouseId { get; set; }
        public string? Notes { get; set; }
        public int? RequestId { get; set; }
        public int? FuelMethodOfComsuptionId { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        public int? DriverId { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public string? UpdatedBy { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public virtual BranchOffices? BranchOffice { get; set; }
        [JsonIgnore]
        public virtual Vehicle? Vehicle { get; set; }
        [JsonIgnore]
        public virtual Driver? Driver { get; set; }
        [JsonIgnore]
        public virtual Road? Road { get; set; }
        [JsonIgnore]
        public virtual Request? Request { get; set; }
        [JsonIgnore]
        public virtual Dispenser? Dispenser { get; set; }
        [JsonIgnore]
        public virtual WareHouse? WareHouse { get; set; }
        [JsonIgnore]
        public virtual WareHouse? ToWareHouse { get; set; }
        [JsonIgnore]
        public virtual ArticleDataMaster? ArticleDataMaster { get; set; }
    }
}
