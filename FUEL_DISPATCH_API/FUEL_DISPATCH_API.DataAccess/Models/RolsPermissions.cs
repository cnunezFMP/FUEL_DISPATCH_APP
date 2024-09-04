using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class RolsPermissions
    {
        public int? RolId { get; set; }
        public int? PermissionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public int? CompanyId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        [JsonIgnore] public virtual Role? Role { get; set; }
        [JsonIgnore] public virtual Companies? Company { get; set; }
        [JsonIgnore] public virtual Permission? Permission { get; set; }
    }
}
