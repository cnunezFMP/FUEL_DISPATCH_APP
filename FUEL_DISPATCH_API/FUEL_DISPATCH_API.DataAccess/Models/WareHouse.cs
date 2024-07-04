using FUEL_DISPATCH_API.Utils.Constants;
using System.Text.Json.Serialization;
#nullable disable
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class WareHouse
    {
        [JsonIgnore]
        public virtual BranchOffices? BranchOffice { get; set; }

        public int? BranchOfficeId { get; set; }

        public string Code { get; set; }

        [JsonIgnore]
        public virtual Companies Company { get; set; }

        public int? CompanyId { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        public string FullDirection { get; set; }

        public int Id { get; set; }

        public decimal MaxCapacity { get; set; }

        public decimal MinCapacity { get; set; }

        public string Name { get; set; }

        public string Representative { get; set; }

        public string Status { get; set; } = ValidationConstants.ActiveStatus;

        [JsonIgnore]
        public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

        [JsonIgnore]
        public virtual ICollection<WareHouseMovement> ToWareHouseMovements
        {
            get;
            set;
        } = new List<WareHouseMovement>();

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        public string? UpdatedBy { get; set; }

        [JsonIgnore]
        public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = new List<WareHouseMovement>();
    }
}
