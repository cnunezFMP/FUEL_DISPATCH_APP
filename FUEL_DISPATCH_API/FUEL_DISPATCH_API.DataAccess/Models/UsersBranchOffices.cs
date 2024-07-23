using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class UsersBranchOffices
    {
        [Required] public int BranchOfficeId { get; set; }
        [Required] public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        [JsonIgnore]public virtual BranchOffices? BranchOffice { get; set; }
        [JsonIgnore]public virtual User? User{ get; set; }
    }
}
