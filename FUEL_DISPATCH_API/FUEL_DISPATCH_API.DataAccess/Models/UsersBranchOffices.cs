using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class UsersBranchOffices
    {
        public int BranchOfficeId { get; set; }
        public int CompanyId { get; set; }
        [Required] public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }
        [JsonIgnore] public virtual Companies? Company { get; set; }
        [JsonIgnore] public virtual User? User { get; set; }
    }
}
