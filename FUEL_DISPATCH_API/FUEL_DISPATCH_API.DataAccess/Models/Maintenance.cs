using FUEL_DISPATCH_API.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        [Required] public DateTime Date { get; set; }
        public string? Technician { get; set; }
        public string? Code { get; set; }
        [Required] public int VehicleId { get; set; }
        public decimal? CurrentOdometer { get; set; }
        public string? VehicleVin { get; set; }
        public MaitenanceStatusEnum? Status { get; set; } = MaitenanceStatusEnum.NotStarted;
        [JsonIgnore] public Vehicle? Vehicle { get; set; }
        public ICollection<MaintenanceDetails> Details { get; set; } = [];
    }

}
