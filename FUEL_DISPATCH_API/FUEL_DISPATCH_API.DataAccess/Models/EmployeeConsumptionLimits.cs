using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class EmployeeConsumptionLimits
    {
        public int? DriverId { get; set; }
        public int? MethodOfComsuptionId { get; set; }
        public decimal? LimitAmount { get; set; }
        public decimal? CurrentAmount { get; set; }
        [JsonIgnore] public virtual Driver? Driver { get; set; }
        [JsonIgnore] public virtual DriverMethodOfComsuption? DriverMethodOfComsuption { get; set; }
    }
}
