using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public partial class WareHouseMovement
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int RoadId { get; set; }  
        public int ItemId { get; set; }  
        public int BranchOfficeId { get; set; }   
        public int DispenserId { get; set; } 
        public string Type { get; set; }
        public decimal Qty { get; set; }
        public decimal Odometer { get; set; }
        public int WareHouseId { get; set; }
        public string? Notes { get; set; }
        public int? RequestId { get; set; }  
        public string? CreatedBy { get; set; }
        public int DriverId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public virtual BranchOffices BranchOffice { get; set; }
        [JsonIgnore]
        public Vehicle Vehicle { get; set; }  
        [JsonIgnore]
        public Driver Driver { get; set; }  
        [JsonIgnore]
        public Road Road { get; set; }  
        [JsonIgnore]
        public Request Request { get; set; }  
        [JsonIgnore]
        public Dispenser Dispenser { get; set; } 
        [JsonIgnore]
        public WareHouse WareHouse { get; set; }
        [JsonIgnore]
        public ArticleDataMaster ArticleDataMaster { get; set; }
    }
}
