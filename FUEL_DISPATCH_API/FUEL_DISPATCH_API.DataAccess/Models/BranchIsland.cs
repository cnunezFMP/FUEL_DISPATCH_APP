using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class BranchIsland
    {
        public int Id { get; set; }
        [Required] public string? Code { get; set; }
        public int? BranchOfficeId { get; set; }
        public int? CompanyId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        //[JsonIgnore] public virtual ICollection<Dispenser> Dispensers { get; set; } = new List<Dispenser>();

        [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
        [JsonIgnore] public virtual Companies? Company { get; set; }

    }
}
