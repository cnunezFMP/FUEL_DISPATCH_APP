using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public partial class Store
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullDirection { get; set; }
        public string BranchOfficeId { get; set; }
        public decimal Disponibility { get; set; }
        public decimal MaxCapacity { get; set; }
        public decimal  MinCapacity { get; set; }
        public virtual BranchOffices BranchOffice { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<Dispatch> Dispatches { get; set; } = new List<Dispatch>();
        public virtual ICollection<StoreMovement> StoreMovements { get; set; } = new List<StoreMovement>(); 
    }
}
