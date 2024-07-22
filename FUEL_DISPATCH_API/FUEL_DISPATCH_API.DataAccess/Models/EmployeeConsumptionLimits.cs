using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class EmployeeConsumptionLimits
    {
        [Required] public int DriverId { get; set; }
        [Required] public int DriverMethodOfComsuptionId { get; set; }
        [Required] public decimal? LimitAmount { get; set; }
        [Required] public decimal? CurrentAmount { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        [JsonIgnore] public virtual Driver Driver { get; set; }
        [JsonIgnore] public virtual DriverMethodOfComsuption DriverMethodOfComsuption { get; set; }
    }
}
