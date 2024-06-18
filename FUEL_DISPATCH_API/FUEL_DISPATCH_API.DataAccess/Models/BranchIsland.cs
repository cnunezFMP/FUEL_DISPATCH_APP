using System.Text.Json.Serialization;
using System.Xml;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class BranchIsland
    {
        public int Id { get; set; }
        public string? Identification { get; set; }
        public int? BranchOfficeId { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public virtual ICollection<Dispenser> Dispensers { get; set; } = new List<Dispenser>();
        [JsonIgnore]
        public virtual BranchOffices BranchOffice { get; set; }
    }
}
