using FUEL_DISPATCH_API.Utils.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class WareHouse
    {

        [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }

        [Required] public int? BranchOfficeId { get; set; }

        [Required] public string? Code { get; set; }
        [JsonIgnore] public virtual Companies? Company { get; set; }

        public int? CompanyId { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        public string? FullDirection { get; set; }

        public int Id { get; set; }

        [Required] public decimal MaxCapacity { get; set; }

        [Required] public decimal MinCapacity { get; set; }

        [Required] public string? Name { get; set; }

        [Required] public string? Representative { get; set; }

        public string Status { get; set; } = ValidationConstants.ActiveStatus;


        [JsonIgnore] public virtual ICollection<Stock>? Stocks { get; set; } = new List<Stock>();
        [JsonIgnore] public virtual ICollection<WareHouseMovementRequest>? WareHouseMovementRequests { get; set; } = new List<WareHouseMovementRequest>();
        [JsonIgnore] public virtual ICollection<WareHouseMovementRequest>? ToWareHouseMovementRequests { get; set; } = new List<WareHouseMovementRequest>();
        [JsonIgnore] public virtual ICollection<WareHouseMovement>? ToWareHouseMovements { get; set; } = new List<WareHouseMovement>();

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        [JsonIgnore] public virtual ICollection<WareHouseMovement>? WareHouseMovements { get; set; } = new List<WareHouseMovement>();
    }
}
