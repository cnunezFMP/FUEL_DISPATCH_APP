using FUEL_DISPATCH_API.Utils.Constants;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class WareHouse
    {
        public int Id { get; set; } 
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullDirection { get; set; }
        public int? BranchOfficeId { get; set; }
        public decimal MaxCapacity { get; set; }
        public decimal MinCapacity { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; } = ValidationConstants.ActiveStatus;
        public string Representative { get; set; }

        [JsonIgnore]
        public ICollection<WareHouseMovement> WareHouseMovements { get; set; } = new List<WareHouseMovement>();
        [JsonIgnore]
        public BranchOffices BranchOffice { get; set; }
        [JsonIgnore]
        public ICollection<Stock> Stocks {  get; set; } = new List<Stock>();    
    }
}
