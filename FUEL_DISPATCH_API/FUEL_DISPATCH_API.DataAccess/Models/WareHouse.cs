using FUEL_DISPATCH_API.DataAccess.Enums;
using FUEL_DISPATCH_API.Utils.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class WareHouse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
        public int? BranchOfficeId { get; set; }
        [Required] public string? Code { get; set; }
        [JsonIgnore] public virtual Companies? Company { get; set; }
        public int? CompanyId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
        public string? FullDirection { get; set; }
        [Required] public decimal? MaxCapacity { get; set; }
        [Required] public decimal? MinCapacity { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public string? Representative { get; set; }
        public ActiveInactiveStatussesEnum? Status { get; set; } = ActiveInactiveStatussesEnum.Active;
        [JsonIgnore] public virtual ICollection<Stock>? Stocks { get; set; } = new List<Stock>();
        [JsonIgnore] public virtual ICollection<WareHouseMovementRequest>? WareHouseMovementRequests { get; set; } = [];
        [JsonIgnore] public virtual ICollection<WareHouseMovementRequest>? ToWareHouseMovementRequests { get; set; } = [];
        [JsonIgnore] public virtual ICollection<WareHouseMovement>? ToWareHouseMovements { get; set; } = [];
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        [JsonIgnore] public virtual ICollection<WareHouseMovement>? WareHouseMovements { get; set; } = [];
    }
}
