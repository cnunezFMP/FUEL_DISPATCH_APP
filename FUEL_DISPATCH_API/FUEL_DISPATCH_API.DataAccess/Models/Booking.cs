using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required] public int? VehicleId { get; set; }
        [Required] public int? DriverId { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchOfficeId { get; set; }
        [Required] public DateTime? SpecificDate { get; set; }
        public DateTime? ToSpecificDate { get; set; }
        [Required] public DateTime? PickUpDate { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? Notes { get; set; }

        [JsonIgnore] public virtual Vehicle? Vehicle { get; set; }
        [JsonIgnore] public virtual Companies? Company { get; set; }
        [JsonIgnore] public virtual BranchOffices? BranchOffice { get; set; }

        [JsonIgnore] public virtual Driver? Driver { get; set; }
    }
}
